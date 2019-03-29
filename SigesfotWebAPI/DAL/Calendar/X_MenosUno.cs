using BE.Common;

namespace DAL.Calendar
{
    public class X_MenosUno : OperatorX
    {
        public override int IsRequired(int pacientAge, int analyzeAge, int pacientGender, int analyzeGender)
        {
            if ((int)Enumeratores.GenderConditional.Ambos == analyzeGender || pacientGender == analyzeGender)
                return (int) Enumeratores.SiNo.Si;
           
            return (int)Enumeratores.SiNo.No;
        }
    }
}