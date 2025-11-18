using UnityEngine;
using System.Collections;


    /// <summary>
    /// Helper class used to describe a grid index including an X and Y position.
    /// Used by all pathfinding methods to describe location in the search space.
    /// </summary>
    public class PIndex
    {
        // Private
        private short x = 0;
        private short y = 0;

        // Public
        /// <summary>
        /// Represents an index instance that is pre-initialized to 0, 0.
        /// </summary>
        public static readonly PIndex zero = new PIndex(0, 0);

        // Properties
        /// <summary>
        /// The X component of the index.
        /// </summary>
        public int X
        {
            get { return x; }
        }

        /// <summary>
        ///  The Y component of the index.
        /// </summary>
        public int Y
        {
            get { return y; }
        }

        // Constructor
        /// <summary>
        /// Default COnstructor.
        /// </summary>
        public PIndex() { }

        /// <summary>
        /// Parameter constructor accepting short values.
        /// </summary>
        /// <param name="x">The X component as a short</param>
        /// <param name="y">The Y component as a short</param>
        public PIndex(short x, short y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Parameter constructor accepting integer values.
        /// </summary>
        /// <param name="x">The X component as an integer</param>
        /// <param name="y">The Y component as an integer</param>
        public PIndex(int x, int y)
        {
            this.x = (short)x;
            this.y = (short)y;
        }

        /// <summary>
        /// Parameter constructor accepting another instance.
        /// </summary>
        /// <param name="other">The <see cref="PIndex"/> instance to copy</param>
        public PIndex(PIndex other)
        {
            this.x = other.x;
            this.y = other.y;
        }

        // Methods
        /// <summary>
        /// Overriden equals method.
        /// </summary>
        /// <param name="obj">The object to compare against</param>
        /// <returns>True if the specified object is equal to this object</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if ((obj is PIndex) == false)
                return false;

            PIndex pIndex = obj as PIndex;

            if (this.x == pIndex.x &&
                this.y == pIndex.y)
                return true;

            return false;
        }

        /// <summary>
        /// Overriden hash code method.
        /// </summary>
        /// <returns>The hash code for this <see cref="PIndex"/></returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();
        }

        /// <summary>
        /// Overriden to string method.
        /// </summary>
        /// <returns>This <see cref="PIndex"/> represented as a string value</returns>
        public override string ToString()
        {
            return string.Format("Index = ({0}, {1})", x, y);
        }

        /// <summary>
        /// Addition operator.
        /// </summary>
        /// <param name="a">The first <see cref="PIndex"/></param>
        /// <param name="b">The second <see cref="PIndex"/></param>
        /// <returns>The resulting <see cref="PIndex"/></returns>
        public static PIndex operator+(PIndex a, PIndex b)
        {
            // Add index values
            return new PIndex(a.x + b.x, a.y + b.y);
        }

        /// <summary>
        /// Subtraction operator.
        /// </summary>
        /// <param name="a">The first <see cref="PIndex"/></param>
        /// <param name="b">The second <see cref="PIndex"/></param>
        /// <returns>The resulting <see cref="PIndex"/></returns>
        public static PIndex operator-(PIndex a, PIndex b)
        {
            // Subtract index values
            return new PIndex(a.x - b.x, a.y - b.y);
        }
    }

