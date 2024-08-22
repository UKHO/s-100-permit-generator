namespace UKHO.S100PermitService.Common.Security
{
    public class S100ProductSpecification
    {
        private readonly int nr;

        public S100ProductSpecification(int nr)
        {
            if (nr < 100 || nr > 999)
            {
                throw new ArgumentException("Illegal S-100 product specification number: " + nr);
            }
            this.nr = nr;
        }




    }

}
