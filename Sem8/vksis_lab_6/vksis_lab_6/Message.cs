namespace vksis_lab_6
{
    public class Message
    {
        public SFS SFS { get; set; }
        public FCSCoverage FcsCoverage { get; set; }
        public EFS EFS { get; set; }

        public Message()
        {
            SFS = new SFS();
            FcsCoverage = new FCSCoverage();
            EFS = new EFS();
        }
    }

    #region SFS

    public class SFS
    {
        public SD SD { get; set; }
        public AC AC { get; set; }
        public FC FC { get; set; }

        public SFS()
        {
            SD = new SD();
            AC = new AC();
            FC = new FC();
        }
    }

    public class SD
    {

    }

    public class AC
    {
        public int P;
        public int T;
        public int M;
        public int R;
    }

    public class FC
    {
        
    }

    #endregion

    #region FCSCoverage

    public class FCSCoverage
    {
        public int DA { get; set; }
        public int SA { get; set; }
        public RI RI { get; set; }
        public string INFO { get; set; }
        public FCS FCS { get; set; }

        public FCSCoverage()
        {
            RI = new RI();
            FCS = new FCS();
        }
    }

    public class RI
    {
        
    }

    public class FCS
    {
        
    }

    #endregion

    #region EFS

    public class EFS
    {
        public ED ED { get; set; }
        public FS FS { get; set; }
        public IFG IFG { get; set; }

        public EFS()
        {
            ED = new ED();
            FS = new FS();
            IFG = new IFG();
        }
    }

    public class ED
    {
    }

    public class FS
    {
        public int A;
        public int C;
        public int r;   
    }

    public class IFG
    {
        
    }

    #endregion
}
