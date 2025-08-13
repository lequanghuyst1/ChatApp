import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
// import './index.css'
import { BrowserRouter } from "react-router-dom";
import { Suspense } from "react";
import App from "./App";

createRoot(document.getElementById("root")!).render(
  // <StrictMode>
  <BrowserRouter>
    <Suspense>
      <App />
    </Suspense>
  </BrowserRouter>
  // </StrictMode>,
);
