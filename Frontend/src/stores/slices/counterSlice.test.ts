import { configureStore } from '@reduxjs/toolkit';
import counterReducer, {
  CounterState,
  decrement,
  increment,
  incrementByAmount,
} from './counterSlice';
import { AppDispatch } from '../store';

interface RootState {
  counter: CounterState;
}

describe('counterSlice', () => {
  let store: ReturnType<typeof configureStore<RootState>>;
  let dispatch: AppDispatch;

  beforeEach(() => {
    store = configureStore<RootState>({
      reducer: {
        counter: counterReducer,
      },
    });
    dispatch = store.dispatch;
    jest.clearAllMocks();
  });

  test('increment', () => {
    const oldValue = store.getState().counter.value;
    expect(oldValue).toBe(0); // Lấy state ban đầu
    dispatch(increment());
    const newValue = store.getState().counter.value;
    expect(newValue).toBe(oldValue + 1); // Lấy state mới sau dispatch
  });

  test('decrement', () => {
    const oldValue = store.getState().counter.value;
    expect(oldValue).toBe(0);
    dispatch(decrement());
    const newValue = store.getState().counter.value;
    expect(newValue).toBe(-1);
  });

  test('incrementByAmount', () => {
    const oldValue = store.getState().counter.value;
    expect(oldValue).toBe(0);
    dispatch(incrementByAmount(5));
    const newValue = store.getState().counter.value;
    expect(newValue).toBe(oldValue + 5);
  });
});
