// import "./App.css";
import Router from './routes/sections';
import ThemeProvider from './theme';
import { useAppDispatch } from './stores/hook';
import { initialize } from './stores/slices/authSlice';
import { useEffect } from 'react';

function App() {
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(initialize());
  }, [dispatch]);

  return (
    <ThemeProvider>
      <Router />
    </ThemeProvider>
  );
}

export default App;
