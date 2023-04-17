import { useRoutes } from "react-router";
import { StartScene } from "src/scene/start";

export const AppRoutes = () => {
  const element = useRoutes([{ path: "/", element: <StartScene /> }]);
  return <>{element}</>;
};
