namespace DryIoc
{
    public partial class Container
    {
        private static Container instance;
        public static Container Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new Container(rules => rules.WithDefaultReuse(Reuse.Singleton));
                }
                else if (instance.IsDisposed)
                {
                    instance = new Container(rules => rules.WithDefaultReuse(Reuse.Singleton));
                }

                return instance;
            }
        }
    }
}
