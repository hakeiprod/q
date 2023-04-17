import { render } from "@reactunity/renderer";
import "./index.scss";
import { AppRoutes } from "./routes";
import { AppProviders } from "./providers/app";

function App() {
  return (
    <AppProviders>
      <AppRoutes />
    </AppProviders>
  );
}

render(<App />);
