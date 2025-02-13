namespace MVCSample.Infrastructure.Contexts
{
    public static class GlobalContext
    {
        private static Context _context;

        public static void Construct(Context context)
        {
            _context = context;
        }

        public static Context TempGetContext()
        {
            return _context;
        }
    }
}
