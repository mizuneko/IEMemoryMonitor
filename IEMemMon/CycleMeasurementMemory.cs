using System;
using System.Timers;

namespace IEMemMon
{
    public class CycleMeasurementMemory : IDisposable
    {
        #region Fields
        private Action<object, MemoryEventArgs> _elapsed;
        private Timer timer;

        public Action<object, MemoryEventArgs> Elapsed
        {
            set
            {
                ArgumentUtility.NullCheck(value);
                this._elapsed = value;
            }
            get { return this._elapsed; }
        }
        #endregion Fields

        #region Constructors
        public CycleMeasurementMemory()
        {
            _elapsed = (object o, MemoryEventArgs e) =>
            {
                Console.WriteLine(Environment.WorkingSet + "byte");
            };
        }
        #endregion Constructors

        #region Public Methods
        public void Start(TimeSpan interval)
        {
            if (interval < TimeSpan.FromMilliseconds(10))
            {
                throw new ArgumentOutOfRangeException(
                    "interval", "Can not specify less than 10 milli second.");
            }
            if (this.timer != null)
            {
                return;
            }
            this.timer = new Timer(interval.TotalMilliseconds);
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Start();
        }

        public void Stop()
        {
            using (this.timer) { }
            this.timer = null;
        }

        public void Dispose()
        {
            using (this.timer) { }
            this._elapsed = null;
        }

        public static MemoryEventArgs Measurement()
        {
            return new MemoryEventArgs(DateTime.Now, Environment.WorkingSet);
        }
        #endregion Public Methods

        #region Private Methods
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timer.Stop();
            this._elapsed(this, Measurement());
            this.timer.Start();
        }
        #endregion Private Methods
    }
}
