import { render } from "@reactunity/renderer";
import "./index.scss";
import { AppRoutes } from "./routes";
import { AppProviders } from "./providers/app";

const App = () => (
  <AppProviders>
    <AppRoutes />
  </AppProviders>
);

render(<App />);
