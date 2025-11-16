import { createRoot } from 'react-dom/client';
// import './index.css'
import { BrowserRouter } from 'react-router-dom';
import { Suspense } from 'react';
import App from './app';
import { Provider } from 'react-redux';
import { store } from './stores/store';

createRoot(document.getElementById('root')!).render(
  <Provider store={store}>
    <BrowserRouter>
      <Suspense>
        <App />
      </Suspense>
    </BrowserRouter>
  </Provider>
);
