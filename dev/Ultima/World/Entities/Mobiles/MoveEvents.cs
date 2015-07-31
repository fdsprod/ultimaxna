using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimaXNA.Ultima.World.Entities.Mobiles
{
    // This class queues moves and maintains the fastwalk key and current sequence value.
    class MoveEvents
    {
        private int m_lastSequenceAck;
        private int m_sequenceQueued;
        private int m_sequenceNextSend;
        private int m_FastWalkKey;
        MoveEvent[] m_history;

        public bool SlowSync
        {
            get
            {
                if (m_sequenceNextSend > m_lastSequenceAck + 4)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public MoveEvents()
        {
            ResetMoveSequence();
        }

        public void ResetMoveSequence()
        {
            m_sequenceQueued = 0;
            m_lastSequenceAck = -1;
            m_sequenceNextSend = 0;
            m_FastWalkKey = new Random().Next(int.MinValue, int.MaxValue);
            m_history = new MoveEvent[256];
        }

        public void AddMoveEvent(int x, int y, int z, int facing, bool createdByPlayerInput)
        {
            MoveEvent moveEvent = new MoveEvent(x, y, z, facing, m_FastWalkKey);
            moveEvent.CreatedByPlayerInput = createdByPlayerInput;

            m_history[m_sequenceQueued] = moveEvent;

            m_sequenceQueued += 1;
            if (m_sequenceQueued > byte.MaxValue)
                m_sequenceQueued = 1;
        }

        public MoveEvent GetMoveEvent(out int sequence)
        {
            if (m_history[m_sequenceNextSend] != null)
            {
                MoveEvent m = m_history[m_sequenceNextSend];
                m_history[m_sequenceNextSend] = null;
                sequence = m_sequenceNextSend;
                m_sequenceNextSend++;
                if (m_sequenceNextSend > byte.MaxValue)
                    m_sequenceNextSend = 1;
                return m;
            }
            else
            {
                sequence = 0;
                return null;
            }
        }

        public void MoveRequestAcknowledge(int sequence)
        {
            m_history[sequence] = null;
            m_lastSequenceAck = sequence;
        }

        public void MoveRequestReject(int sequence, out int x, out int y, out int z, out int facing)
        {
            if (m_history[sequence] != null)
            {
                MoveEvent e = m_history[sequence];
                x = e.X;
                y = e.Y;
                z = e.Z;
                facing = e.Facing;
            }
            else
            {
                x = y = z = facing = -1;
            }
            ResetMoveSequence();
        }
    }

    class MoveEvent
    {
        public bool CreatedByPlayerInput = false;
        public readonly int X, Y, Z, Facing, Fastwalk;
        public MoveEvent(int x, int y, int z, int facing, int fastwalk)
        {
            X = x;
            Y = y;
            Z = z;
            Facing = facing;
            Fastwalk = fastwalk;
        }
    }
}
