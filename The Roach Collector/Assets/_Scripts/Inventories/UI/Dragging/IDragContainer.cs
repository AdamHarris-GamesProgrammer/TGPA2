using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harris.Core.UI.Dragging
{
    /// <summary>
    /// Acts both as a source and destination for dragging.
    /// </summary>
    /// <typeparam name="T">The type that represents the item being dragged.</typeparam>
    public interface IDragContainer<T> : IDragDestination<T>, IDragSource<T> where T : class
    {
    }
}