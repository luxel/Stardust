namespace Stardust
{
    using UnityEngine;
    using System.Collections;
    using System.Diagnostics;

    /// <summary>
    /// A simple timer wraps around Stopwatch class, without using threading or addtional system resources. With a given interval, this could be used to checked periodically to decide whether we should invoke certain function.
    /// </summary>
    public class TimeKeeper
    {
        private Stopwatch m_stopwatch;
        /// <summary>
        /// Interval in milliseconds
        /// </summary>
        private long m_interval;

        private bool m_shouldExecute = false;
        /// <summary>
        /// Checks the timer whether we should execute. If auto reset, the timer will be reset when this returns true.
        /// </summary>
        public bool ShouldExecute
        {
            get
            {
                bool execute = false;
                if (m_shouldExecute)
                {
                    execute = true;
                }
                else
                {
                    if (m_stopwatch.ElapsedMilliseconds >= m_interval)
                    {
                        execute = true;
                    }
                }
                if (execute && AutoReset)
                {
                    Reset();
                }
                return execute;
            }
            set
            {
                m_shouldExecute = true;
            }
        }

        /// <summary>
        /// Whether the timer will auto reset when ShouldExecute returns true.
        /// </summary>
        public bool AutoReset { get; set; }

        /// <summary>
        /// Creates a TimeKeeper with interval in seconds. 
        /// </summary>
        /// <param name="intervalSeconds">Interval in seconds.</param>
        /// <param name="startsImmediately">If true, the timer will be started immediately after the instance is created.</param>
        public TimeKeeper(float intervalSeconds, bool startsImmediately = false) : this((long)(intervalSeconds * 1000), startsImmediately)
        {
        }
        /// <summary>
        /// Creates a TimeKeeper with interval in milliseconds. 
        /// </summary>
        /// <param name="intervalMilliseconds">Interval in milliseconds.</param>
        /// <param name="startsImmediately">If true, the timer will be started immediately after the instance is created.</param>
        public TimeKeeper(long intervalMilliseconds, bool startsImmediately = false)
        {
            m_interval = intervalMilliseconds;
            m_stopwatch = new Stopwatch();
            if (startsImmediately)
            {
                Start();
            }
        }
        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            m_stopwatch.Start();
        }
        /// <summary>
        /// Resets the timer and wait for the next trigger (after the interval)
        /// </summary>
        public void Reset()
        {
            m_shouldExecute = false;
            m_stopwatch.Reset();
            m_stopwatch.Start();
        }
    }
}