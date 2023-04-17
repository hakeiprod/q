import { PropsWithChildren } from "react";
import { MemoryRouter } from "react-router";

export const AppProviders = ({ children }: PropsWithChildren) => {
  return <MemoryRouter>{children}</MemoryRouter>;
};
