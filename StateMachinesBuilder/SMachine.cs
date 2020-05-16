using System;
using System.Collections.Generic;
using System.Threading;

namespace StateMachinesBuilder
{
    /// <summary>
    /// Base class for any state machines.
    /// </summary>
    /// <typeparam name="TIn">Type of input argument</typeparam>
    /// <typeparam name="TOut">Type of items that this machine produces.</typeparam>
    public abstract class SMachine<TIn, TOut>
    {
        private State _state;
        private Action<Option<TIn>>? _stateFunc;
        private Action? _endFunc;

        private Stack<TIn>? _returnedInputs;
        private Option<TOut> _outResult;

        private IEnumerator<TIn>? _inputEnumerator;

        public IEnumerable<TOut> Run(IEnumerable<TIn> inputs)
        {
            if (inputs == null)
                throw new ArgumentNullException(nameof(inputs));

            try
            {
                Init(inputs);

                while (_state != State.PostEnd)
                {
                    RunState(GetInput());
                    if (_outResult.HasValue)
                    {
                        yield return GetResult();
                    }
                }
            }
            finally
            {
                Clear();
            }
        }

        protected abstract Action<Option<TIn>> GetStartState();

        protected virtual void Reset() {}

        protected virtual void SetNextState(Action<Option<TIn>> state)
        {
            CheckEndState();
            _stateFunc = state ?? throw new ArgumentNullException(nameof(state));
        }

        protected virtual void SetEndState(Action endState)
        {
            CheckEndState();
            _endFunc = endState ?? throw new ArgumentNullException(nameof(endState));
            Thread.MemoryBarrier();
            _state = State.End;
        }

        protected virtual void YieldResult(TOut outResult) => _outResult = outResult!;

        protected virtual void Return(TIn input) => _returnedInputs!.Push(input);

        private void Init(IEnumerable<TIn> inputs)
        {
            Reset();

            _state = State.Running;
            _stateFunc = GetStartState() ?? throw new InvalidOperationException($"Function {nameof(GetStartState)} returned null.");
            _endFunc = null;

            _returnedInputs = new Stack<TIn>();
            _outResult = Option<TOut>.Empty();

            _inputEnumerator = inputs.GetEnumerator();
        }

        private void RunState(Option<TIn> input)
        {
            switch (_state)
            {
                case State.Running:
                    _stateFunc!(input);
                    break;
                case State.End:
                    _endFunc!();
                    _state = State.PostEnd;
                    break;
                case State.PostEnd:
                    throw new InvalidOperationException($"Internal error. Can't run state when state is {State.PostEnd}.");
                default:
                    throw new InvalidOperationException($"Internal error. State has invalid value: {_state}.");
            }
        }

        private Option<TIn> GetInput()
        {
            if (_returnedInputs!.TryPop(out var result))
                return result!;
            else if (_inputEnumerator!.MoveNext())
                return _inputEnumerator!.Current!;
            else
                return Option<TIn>.Empty();
        }

        private TOut GetResult()
        {
            TOut result = _outResult.Value;
            _outResult = Option<TOut>.Empty();
            return result;
        }

        private void CheckEndState()
        {
            if (_state == State.End)
            {
                throw new InvalidOperationException($"Can't change state when current state is {State.End}.");
            }
        }

        private void Clear()
        {
            _state = default;
            _stateFunc = null;
            _returnedInputs?.Clear();
            _returnedInputs = null;
            _outResult = Option<TOut>.Empty();
            _inputEnumerator?.Dispose();
            _inputEnumerator = default;
        }
    }
}
