using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A waypoint to be used for path planning
/// </summary>
public class Waypoint : MonoBehaviour
{
    /// <summary>
    /// Other Waypoints you can get to from this one with a straight-line path
    /// </summary>
    [NonSerialized]
    public List<Waypoint> Neighbors = new List<Waypoint>();

    /// <summary>
    /// Used in path planning; next closest node to the start node
    /// </summary>
    private Waypoint predecessor;

    /// <summary>
    /// Cached list of all waypoints.
    /// </summary>
    static Waypoint[] AllWaypoints;

    /// <summary>
    /// Compute the Neighbors list
    /// </summary>
    internal void Start()
    {
        if (Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Land"))) {
            Debug.Log(this.gameObject.name);
            Destroy(this.gameObject);
        }
        var position = transform.position;
        if (AllWaypoints == null)
        {
            AllWaypoints = FindObjectsOfType<Waypoint>();
        }

        foreach (var wp in AllWaypoints) 
            if (wp != this && !WaypointBlocked(transform.position, wp.transform.position))
                Neighbors.Add(wp);
    }
    public static bool WaypointBlocked(Vector3 t1, Vector3 t2) {
        return Physics2D.CircleCast(t1, .25f, t2-t1, Vector3.Distance(t1, t2), LayerMask.GetMask("Land"));
    }

    /// <summary>
    /// Visualize the waypoint graph
    /// </summary>
    internal void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, 0.5f);
        foreach (var wp in Neighbors)
            Gizmos.DrawLine(position, wp.transform.position);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(NearestWaypointTo(BehaviorTreeNode.Player.transform.position).transform.position, 1f);
    }

    /// <summary>
    /// Nearest waypoint to specified location that is reachable by a straight-line path.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Waypoint NearestWaypointTo(Vector2 position)
    {
        Waypoint nearest = null;
        var minDist = float.PositiveInfinity;
        for (int i = 0; i < AllWaypoints.Length; i++)
        {
            var wp = AllWaypoints[i];
            var p = wp.transform.position;
            var d = Vector2.Distance(position, p);
            if (d < minDist && !WaypointBlocked(p, position))
            {
                nearest = wp;
                minDist = d;
            }
        }
        return nearest;
    }


    /// <summary>
    /// Returns a series of waypoints to take to get to the specified position
    /// </summary>
    /// <param name="start">Starting position</param>
    /// <param name="end">Desired endpoint</param>
    /// <returns></returns>
    public static List<Waypoint> FindPath(Vector2 start, Vector2 end)
    {
        List<Waypoint> wp =  FindPath(NearestWaypointTo(start), NearestWaypointTo(end));
        return wp;
    }


    /// <param name="start">Starting waypoint</param>
    /// <param name="end">Goal waypoint</param>
    /// <returns></returns>
    static List<Waypoint> FindPath(Waypoint start, Waypoint end)
    {
        if (!start || !end) {
            return null;
        }
        // Do a BFS of the graph
        PriorityQueue q = new PriorityQueue(start.transform.position, end.transform.position);
        foreach (var wp in AllWaypoints)
            wp.predecessor = null;
        q.Enqueue(start);
        Waypoint node;
        while ((node = q.Dequeue()) != end)
        {
            foreach (var n in node.Neighbors)
            {
                if (n.predecessor == null)
                {
                    q.Enqueue(n);
                    n.predecessor = node;
                }
            }
        }

        // Reconstruct the path
        var path = new List<Waypoint>();
        path.Add(node);
        while (node != start)
        {
            node = node.predecessor;
            path.Insert(0, node);
        }
        return path;
    }
    public class PriorityQueue
    {
        private List<Waypoint> data;
        private Vector2 end, start;

        public PriorityQueue(Vector2 start, Vector2 end)
        {   
            this.end = end;
            this.start = start;
            this.data = new List<Waypoint>();
        }

        public void Enqueue(Waypoint item)
        {
        data.Add(item);
        int ci = data.Count - 1; // child index; start at end
        while (ci > 0)
        {
            int pi = (ci - 1) / 2; // parent index
            if (Compare(data[ci],data[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done            Waypoint tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
            ci = pi;
        }
        }

        public Waypoint Dequeue()
        {
        // assumes pq is not empty; up to calling code
        int li = data.Count - 1; // last index (before removal)
        Waypoint frontItem = data[0];   // fetch the front
        data[0] = data[li];
        data.RemoveAt(li);

        --li; // last index (after removal)
        int pi = 0; // parent index. start at front of pq
        while (true)
        {
            int ci = pi * 2 + 1; // left child index of parent
            if (ci > li) break;  // no children so done
            int rc = ci + 1;     // right child
            if (rc <= li && Compare(data[rc],data[ci]) < 0)// if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
            ci = rc;
            if (Compare(data[pi],data[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
            Waypoint tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp; // swap parent and child
            pi = ci;
        }
        return frontItem;
        }

        public Waypoint Peek()
        {
        Waypoint frontItem = data[0];
        return frontItem;
        }

        public int Count()
        {
        return data.Count;
        }

        public override string ToString()
        {
        string s = "";
        for (int i = 0; i < data.Count; ++i)
            s += data[i].ToString() + " ";
        s += "count = " + data.Count;
        return s;
        }
        public float Compare(Waypoint w1, Waypoint w2) {
            float f1 = Vector2.Distance(start, w1.transform.position) + Vector2.Distance(end, w1.transform.position);
            float f2 = Vector2.Distance(start, w2.transform.position) + Vector2.Distance(end, w2.transform.position);
            return f1 - f2;

        }
    }
}