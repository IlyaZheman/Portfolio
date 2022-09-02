using GameScripts.Bot;
using GameScripts.Characters;
using GameScripts.Game;
using GameScripts.Level;
using GameScripts.Player;
using GameScripts.Providers.Interface;
using GameScripts.Providers.Module;
using GameScripts.UI;
using GameScripts.UI.View;
using UnityEngine;
using Zenject;

namespace GameScripts.Misc
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject finishPrefab;
        [SerializeField] private GameObject botPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private LevelConfiguration levelConfiguration;
        
        public override void InstallBindings()
        {
            Container.Bind<PlatformPositionChooser>().FromNew().AsSingle();
            Container.Bind<PlatformSpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LevelProvider>().FromNew().AsSingle();
            Container.Bind<TrackPositionsViewModel>().FromNew().AsSingle();
            Container.Bind<IInputProvider>().To<TapInputProvider>().FromComponentsInHierarchy().AsSingle();
            Container.Bind<GameStarter>().FromNew().AsSingle();
            Container.BindFactory<Finish, FinishFactory>().FromComponentInNewPrefab(finishPrefab);
            Container.BindFactory<CharacterState, BotStateFactory>().FromComponentInNewPrefab(botPrefab);
            Container.BindFactory<CharacterState, PlayerStateFactory>().FromComponentInNewPrefab(playerPrefab);
            Container.Bind<FinishPanelViewModel>().FromNew().AsSingle();
            Container.Bind<TrackPositionsView>().FromInstance(FindObjectOfType<TrackPositionsView>());
            Container.Bind<LevelConfiguration>().FromInstance(levelConfiguration);
        }
    }
}