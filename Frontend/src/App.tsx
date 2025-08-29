// import "./App.css";
import AuthProvider from "./stores/auth";
import Router from "./routes/sections";
import ThemeProvider from "./theme";
import { Provider } from "react-redux";
import { store } from "./stores/store";

function App() {
  return (
    <Provider store={store}>
      <AuthProvider>
        <ThemeProvider>
          <Router />
        </ThemeProvider>
      </AuthProvider>
    </Provider>
  );
}

export default App;
