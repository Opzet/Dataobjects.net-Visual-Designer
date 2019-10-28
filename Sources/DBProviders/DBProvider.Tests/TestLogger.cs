using System.Diagnostics;

namespace DBProvider.Tests
{
    public class TestLogger
    {
        public string Category { get; set; }

        public TestLogger(string category)
        {
            Category = category;
        }

        public void Log(string message, params object[] args)
        {
            message = "  " + message;
            if (!string.IsNullOrEmpty(Category))
            {
                message = string.Format("[{0}] {1}", Category, message);
            }

            if (args == null || args.Length == 0)
            {
                Debug.WriteLine(message);
            }
            else
            {
                Debug.WriteLine(message, args);
            }
        }
    }
}