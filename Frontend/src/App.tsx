// import "./App.css";
import AuthProvider from "./stores/auth";
import Router from "./routes/sections";

function App() {
  return (
    <AuthProvider>
      <Router />
    </AuthProvider>
  );
}

export default App;
