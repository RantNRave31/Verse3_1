using System;
using System.Collections;
using System.Collections.Generic;

using GKYU.CoreLibrary;

namespace GKYU.CollectionsLibrary.Collections
{
    public class CharacterSet
        : IEquatable<CharacterSet>
        , IEnumerable<int>
    {
        public class Range
        {
            public int from, to;
            public Range next;
            public Range(int from, int to) { this.from = from; this.to = to; }
        }

        public Range head;
        public CharacterSet()
        {

        }
        public CharacterSet(CharacterSet characterSet)
        {
            Or(characterSet);
        }
        public CharacterSet(Range range)
        {
            head = range;
        }
        public CharacterSet(int from, int to)
        {
            head = new Range(from, to);
        }
        public CharacterSet(int value)
        {
            head = new Range(value, value);
        }
        public bool this[int i]
        {
            get
            {
                for (Range p = head; p != null; p = p.next)
                    if (i < p.from) return false;
                    else if (i <= p.to) return true; // p.from <= i <= p.to
                return false;
            }
        }
        public void Set(int i)
        {
            Range cur = head, prev = null;
            while (cur != null && i >= cur.from - 1)
            {
                if (i <= cur.to + 1)
                { // (cur.from-1) <= i <= (cur.to+1)
                    if (i == cur.from - 1) cur.from--;
                    else if (i == cur.to + 1)
                    {
                        cur.to++;
                        Range next = cur.next;
                        if (next != null && cur.to == next.from - 1)
                        {
                            cur.to = next.to;
                            cur.next = next.next;
                        };
                    }
                    return;
                }
                prev = cur;
                cur = cur.next;
            }
            Range n = new Range(i, i);
            n.next = cur;
            if (prev == null)
                head = n;
            else
                prev.next = n;
        }
        public CharacterSet Clone()
        {
            CharacterSet s = new CharacterSet();
            Range prev = null;
            for (Range cur = head; cur != null; cur = cur.next)
            {
                Range r = new Range(cur.from, cur.to);
                if (prev == null) s.head = r; else prev.next = r;
                prev = r;
            }
            return s;
        }
        public bool Equals(CharacterSet s)
        {
            Range p = head, q = s.head;
            while (p != null && q != null)
            {
                if (p.from != q.from || p.to != q.to) return false;
                p = p.next; q = q.next;
            }
            return p == q;
        }
        public int Count()
        {
            int n = 0;
            for (Range p = head; p != null; p = p.next) n += p.to - p.from + 1;
            return n;
        }
        public int First()
        {
            if (head != null) return head.from;
            return -1;
        }
        public CharacterSet Or(CharacterSet s)
        {
            for (Range p = s.head; p != null; p = p.next)
                for (int i = p.from; i <= p.to; i++) Set(i);
            return this;
        }
        public CharacterSet And(CharacterSet s)
        {
            CharacterSet x = new CharacterSet();
            for (Range p = head; p != null; p = p.next)
                for (int i = p.from; i <= p.to; i++)
                {
                    if (s[i])
                        x.Set(i);
                }
            head = x.head;
            return this;
        }
        public CharacterSet Subtract(CharacterSet s)
        {
            CharacterSet x = new CharacterSet();
            for (Range p = head; p != null; p = p.next)
                for (int i = p.from; i <= p.to; i++)
                    if (!s[i]) x.Set(i);
            head = x.head;
            return this;
        }
        public bool Includes(CharacterSet s)
        {
            for (Range p = s.head; p != null; p = p.next)
                for (int i = p.from; i <= p.to; i++)
                    if (!this[i]) return false;
            return true;
        }
        public bool Intersects(CharacterSet s)
        {
            for (Range p = s.head; p != null; p = p.next)
                for (int i = p.from; i <= p.to; i++)
                    if (this[i]) return true;
            return false;
        }
        public void Fill()
        {
            head = new Range(char.MinValue, char.MaxValue);
        }

        public IEnumerator<int> GetEnumerator()
        {
            Range currentRange = head;
            while (currentRange != null)
            {
                for (int characterCode = currentRange.from; characterCode <= currentRange.to; characterCode++)
                {
                    yield return characterCode;
                }
                currentRange = currentRange.next;
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
