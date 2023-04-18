import { PropsWithChildren } from "react";
import { MemoryRouter as Router } from "react-router";

export const AppProviders = ({ children }: PropsWithChildren) => (
  <Router>{children}</Router>
);
