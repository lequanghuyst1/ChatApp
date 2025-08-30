// import "./App.css";
import Router from './routes/sections';
import ThemeProvider from './theme';
import { useAppDispatch, useAppSelector } from './stores/hook';
import { initialize } from './stores/slices/authSlice';
import { useEffect } from 'react';

function App() {
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(initialize());
  }, [dispatch]);

  const auth = useAppSelector((state) => state.auth);
  console.log(auth);

  return (
    <ThemeProvider>
      <Router />
    </ThemeProvider>
  );
}

export default App;
