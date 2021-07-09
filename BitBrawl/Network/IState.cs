using System;
using System.Collections.Generic;
using System.Text;

namespace BitBrawl.Network
{
    public interface IState
    {
        /// <summary>
        /// Sets an object's properties based on this state
        /// </summary>
        /// <param name="obj"></param>
        public object FromObject(object obj);

        /// <summary>
        /// Sets this state to an object's properties
        /// </summary>
        /// <param name="obj"></param>
        public void ToObject(object obj, double latency);

        /// <summary>
        /// Determines whether or not the state should be updated to the server
        /// </summary>
        /// <returns><see langword="true"/> if the state should be updated to the server</returns>
        public bool ShouldUpdate(IState previousState);
    }
}
