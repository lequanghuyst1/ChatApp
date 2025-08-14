// import "./App.css";
import AuthProvider from "./stores/auth";
import Router from "./routes/sections";
import ThemeProvider from "./theme";

function App() {
  return (
    <AuthProvider>
      <ThemeProvider>
        <Router />
      </ThemeProvider>
    </AuthProvider>
  );
}

export default App;
