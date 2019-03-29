using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BE.Common;
using BE.Notification;

namespace DAL.Notification
{
    public class NotificationDal
    {
        public string SaveNotification(NotificationDto oNotificationDto, int nodeId, int systemUserId)
        {
            using (var dbContext = new DatabaseContext())
            {
                var objEntity = new NotificationDto();

                objEntity.v_PersonId = oNotificationDto.v_PersonId;
                objEntity.v_OrganizationId = oNotificationDto.v_OrganizationId;
                objEntity.d_NotificationDate = DateTime.Now;
                objEntity.v_Title = oNotificationDto.v_Title;
                objEntity.v_Body = oNotificationDto.v_Body;
                objEntity.i_TypeNotificationId = oNotificationDto.i_TypeNotificationId;
                objEntity.d_ScheduleDate = oNotificationDto.d_ScheduleDate;
                objEntity.i_InsertUserId = systemUserId;
                objEntity.i_IsDeleted = (int)Enumeratores.SiNo.No;  
                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_IsRead = (int)Enumeratores.SiNo.No;
                //Autogeneramos el Pk de la tabla
                objEntity.v_NotificationId = new Common.Utils().GetPrimaryKey(nodeId, 360, "NF");

                dbContext.Notification.Add(objEntity);
                dbContext.SaveChanges();

                return objEntity.v_NotificationId;
            }
        }

        public BoardNotification FilterNotifications(BoardNotification data)
        {
            using (var dbContext = new DatabaseContext())
            {
                int skip = (data.Index - 1) * data.Take;

                string filterPacient = string.IsNullOrWhiteSpace(data.Worker) ? "" : data.Worker;

                var dateStart = DateTime.Parse(data.NotificationDateStart);
                var dateEnd = DateTime.Parse(data.NotificationDateEnd).AddDays(1);
                var title = data.Title == null ? "" : data.Title;
                var query = (from a in dbContext.Notification
                    join b in dbContext.SystemParameter on new { a = a.i_TypeNotificationId.Value, b = 347 } equals new { a = b.i_ParameterId, b = b.i_GroupId }
                    join c in dbContext.SystemUser on a.i_InsertUserId equals c.i_SystemUserId
                    join d in dbContext.Person on c.v_PersonId equals d.v_PersonId
                    join e in dbContext.Organization on a.v_OrganizationId equals e.v_OrganizationId
                    join f in dbContext.Person on a.v_PersonId equals f.v_PersonId
                    join g in dbContext.SystemParameter on new { a = a.i_IsRead.Value, b = 111 } equals new { a = g.i_ParameterId, b = g.i_GroupId }
                    join h in dbContext.SystemParameter on new { a = a.i_StateNotificationId.Value, b = 348 } equals new { a = h.i_ParameterId, b = h.i_GroupId }
                        where (data.TypeNotificationId== -1 || a.i_TypeNotificationId== data.TypeNotificationId) 
                          && ((f.v_FirstName + " " + f.v_FirstLastName + " " + f.v_SecondLastName).Contains(filterPacient)|| f.v_DocNumber.Contains(filterPacient))
                          && (data.OrganizationId == "-1" || a. v_OrganizationId == data.OrganizationId)
                          && (a.d_NotificationDate >= dateStart && a.d_NotificationDate <= dateEnd)
                          && (a.v_Title.Contains(title))
                          && (data.StateNotificationId == -1 || a.i_StateNotificationId == data.StateNotificationId)
                             select new NotificationsBE
                    {
                        PersonId = a.v_PersonId,
                        NotificationId = a.v_NotificationId,
                        Organization = e.v_Name,
                        NotificationDate = a.d_NotificationDate,
                        TypeNotificationId = a.i_TypeNotificationId,
                        TypeNotification = b.v_Value1,
                        Title = a.v_Title,
                        Body = a.v_Body,
                        SystemUserId = a.i_InsertUserId,
                        SystemUser = d.v_FirstName + " " + d.v_FirstLastName,
                        IsRead = a.i_IsRead,
                        Read = g.v_Value1,
                        Worker = f.v_FirstName + " " + f.v_FirstLastName + " " + f.v_SecondLastName,
                        StateNotification = h.v_Value1
                    }).ToList();

                data.TotalRecords = query.Count;
                if (data.Take > 0)
                    query = query.Skip(skip).Take(data.Take).ToList();

                data.list = query;

                return data;

            }
        }

        public List<NotificationsBE> Notifications(string personId)
        {
            using (var dbContext = new DatabaseContext())
            {
                var query = (from a in dbContext.Notification
                            join b in dbContext.SystemParameter on new {a = a.i_TypeNotificationId.Value, b = 347  } equals new  { a = b.i_ParameterId, b = b.i_GroupId  }
                            join c in dbContext.SystemUser on a.i_InsertUserId equals  c.i_SystemUserId
                            join d in dbContext.Person on c.v_PersonId equals d.v_PersonId

                             where a.v_PersonId == personId 
                                  && a.i_IsDeleted == (int)Enumeratores.SiNo.No
                            select new NotificationsBE
                            {
                                NotificationId = a.v_NotificationId,
                                TypeNotificationId = a.i_TypeNotificationId,
                                TypeNotification = b.v_Value1,
                                Title = a.v_Title,
                                Body = a.v_Body,
                                NotificationDate = a.d_NotificationDate,
                                SystemUserId = a.i_InsertUserId,
                                SystemUser = d.v_FirstName + " " + d.v_FirstLastName,
                                IsRead = a.i_IsRead
                            }).OrderByDescending(p => p.NotificationDate).ToList();

                var result = (from a in query
                    select new NotificationsBE
                    {
                        NotificationId = a.NotificationId,
                        TypeNotificationId = a.TypeNotificationId,
                        TypeNotification = a.TypeNotification,
                        Title = a.Title,
                        Body = a.Body,
                        NotificationDate = a.NotificationDate,
                        NotificationDateString = a.NotificationDate.ToString(),
                        SystemUserId = a.SystemUserId,
                        SystemUser = a.SystemUser,
                        IsRead = a.IsRead

                    }).ToList();

                return result;
            }
        }

        public NotificationsBE GetNotification(string notificationId)
        {
            using (var ctx = new DatabaseContext())
            {
                var query = (from a in ctx.Notification
                    join b in ctx.SystemParameter on new { a = a.i_TypeNotificationId.Value, b = 347 } equals new { a = b.i_ParameterId, b = b.i_GroupId }
                    join c in ctx.SystemUser on a.i_InsertUserId equals c.i_SystemUserId
                    join d in ctx.Person on c.v_PersonId equals d.v_PersonId
                    where a.v_NotificationId == notificationId && a.i_IsDeleted == (int)Enumeratores.SiNo.No
                    select new NotificationsBE
                    {
                        TypeNotificationId = a.i_TypeNotificationId,
                        TypeNotification = b.v_Value1,
                        Title = a.v_Title,
                        Body = a.v_Body,
                        NotificationDate = a.d_NotificationDate,
                        ScheduleDate = a.d_ScheduleDate,
                        SystemUserId = a.i_InsertUserId,
                        SystemUser = d.v_FirstName + " " + d.v_FirstLastName,
                        IsRead = a.i_IsRead
                    }).ToList();


                var result = (from a in query
                    select new NotificationsBE
                    {
                        TypeNotificationId =  a.TypeNotificationId,
                        TypeNotification = a.TypeNotification,
                        Title = a.Title,
                        Body = a.Body,
                        NotificationDate = a.NotificationDate,
                        NotificationDateString = a.NotificationDate.ToString(),
                        ScheduleDate = a.ScheduleDate,
                        ScheduleDateString = a.ScheduleDate.ToString(),
                        SystemUserId = a.SystemUserId,
                        SystemUser = a.SystemUser,
                        IsRead = a.IsRead
                    }).FirstOrDefault();

                return result;
            }
        }

        public List<SendNotificationBE> SendNotification(string personId, string notificationId)
        {
            using (var dbContext = new DatabaseContext())
            {
                var query = (from a in dbContext.Notification
                            join b in dbContext.Subscription on a.v_PersonId equals  b.v_PersonId
                            where a.v_PersonId == personId 
                                  && a.i_IsDeleted == (int)Enumeratores.SiNo.No
                                  && a.i_IsRead == (int)Enumeratores.SiNo.No
                             select new SendNotificationBE
                                {
                                    Subs = b.v_Subs,
                                    Title = a.v_Title,
                                    Message = a.v_Body
                                }).ToList();

                    //DeleteNotification(notificationId);
                return query;
            }
        }

        public bool ReadNotification(string notificationId)
        {
            using (var ctx = new DatabaseContext())
            {
                var objEntity = (from a in ctx.Notification where a.v_NotificationId == notificationId select a)
                    .FirstOrDefault();

                if (objEntity == null) return false;

                objEntity.i_IsRead = (int)Enumeratores.SiNo.Si;

                ctx.SaveChanges();
                return true;
            }
        }

        public void DeleteNotification(string notificationId)
        {
            using (var dbContext = new DatabaseContext())
            {
                var objEntity = (from a in dbContext.Notification
                    where a.v_NotificationId == notificationId
                    select a).FirstOrDefault();
                objEntity.i_IsDeleted = (int)Enumeratores.SiNo.No;
                objEntity.d_UpdateDate = DateTime.Now;
                dbContext.SaveChanges();
            }
        }
    }
}
