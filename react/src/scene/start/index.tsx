export const StartScene = () => {
  const handleClick = () =>
    Interop.UnityEngine.SceneManagement.SceneManager.LoadScene(2);
  return (
    <view>
      <text>start scene</text>
      <text>{`Go to <color=red>src/index.tsx</color> to edit this file`}</text>
      <button onClick={handleClick}>start</button>
    </view>
  );
};
