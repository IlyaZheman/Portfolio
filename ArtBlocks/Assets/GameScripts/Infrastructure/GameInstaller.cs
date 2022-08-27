using GameScripts.Game;
using UnityEngine;
using Zenject;

namespace GameScripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameBootstrapper gameBootstrapper;
        [SerializeField] private GameBoardState gameBoardState;

        public override void InstallBindings()
        {
            Container.Bind<GameBootstrapper>().FromInstance(gameBootstrapper);
            Container.Bind<GameBoardState>().FromInstance(gameBoardState);
        }
    }
}