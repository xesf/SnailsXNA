
using System;
namespace TwoBrainsGames.Snails.StageObjects
{
    public class StageObjectFactory
    {
        delegate StageObject CreateObjectDelegate();

        static CreateObjectDelegate[] _constructors;

        public static void Initialize()
        {
            _constructors = new CreateObjectDelegate[(int)StageObjects.StageObjectType.Last + 1];
            _constructors[(int)StageObjects.StageObjectType.Snail] = StageObjectFactory.CreateSnail;
            _constructors[(int)StageObjects.StageObjectType.StageEntrance] = StageObjectFactory.CreateStageEntrance;
            _constructors[(int)StageObjects.StageObjectType.StageExit] = StageObjectFactory.CreateStageExit;
            _constructors[(int)StageObjects.StageObjectType.Apple] = StageObjectFactory.CreateApple;
            _constructors[(int)StageObjects.StageObjectType.Vitamin] = StageObjectFactory.CreateVitamin;
            _constructors[(int)StageObjects.StageObjectType.SnailCounter] = StageObjectFactory.CreateSnailCounter;
            _constructors[(int)StageObjects.StageObjectType.Dynamite] = StageObjectFactory.CreateDynamite;
            _constructors[(int)StageObjects.StageObjectType.Box] = StageObjectFactory.CreateBox;
            _constructors[(int)StageObjects.StageObjectType.Copper] = StageObjectFactory.CreateCopper;
            _constructors[(int)StageObjects.StageObjectType.Explosion] = StageObjectFactory.CreateExplosion;
            _constructors[(int)StageObjects.StageObjectType.Trampoline] = StageObjectFactory.CreateTrampoline;
            _constructors[(int)StageObjects.StageObjectType.Spikes] = StageObjectFactory.CreateSpikes;
            _constructors[(int)StageObjects.StageObjectType.TriggerSwitch] = StageObjectFactory.CreateTriggerSwitch;
            _constructors[(int)StageObjects.StageObjectType.TeleportEntrance] = StageObjectFactory.CreateTeleportEntrance;
            _constructors[(int)StageObjects.StageObjectType.TeleportExit] = StageObjectFactory.CreateTeleportExit;
            _constructors[(int)StageObjects.StageObjectType.PickableObject] = StageObjectFactory.CreatePickableObject;
            _constructors[(int)StageObjects.StageObjectType.Prop] = StageObjectFactory.CreateProp;
            _constructors[(int)StageObjects.StageObjectType.Fire] = StageObjectFactory.CreateFire;
            _constructors[(int)StageObjects.StageObjectType.Salt] = StageObjectFactory.CreateSalt;
            _constructors[(int)StageObjects.StageObjectType.DynamiteBox] = StageObjectFactory.CreateDynamiteBox;
            _constructors[(int)StageObjects.StageObjectType.DynamiteBoxTriggered] = StageObjectFactory.CreateDynamiteBoxTriggered;
            _constructors[(int)StageObjects.StageObjectType.SnailSacrifice] = StageObjectFactory.CreateSnailSacrifice;
            _constructors[(int)StageObjects.StageObjectType.SnailKing] = StageObjectFactory.CreateSnailKing;
            _constructors[(int)StageObjects.StageObjectType.PopUpBox] = StageObjectFactory.CreatePopUpBox;
            _constructors[(int)StageObjects.StageObjectType.Lamp] = StageObjectFactory.CreateLamp;
            _constructors[(int)StageObjects.StageObjectType.FlameLight] = StageObjectFactory.CreateFlameLight;
            _constructors[(int)StageObjects.StageObjectType.Water] = StageObjectFactory.CreateWater;
            _constructors[(int)StageObjects.StageObjectType.Crystal] = StageObjectFactory.CreateCrystal;
            _constructors[(int)StageObjects.StageObjectType.StageProp] = StageObjectFactory.CreateStageProp;
            _constructors[(int)StageObjects.StageObjectType.InformationSign] = StageObjectFactory.CreateInformationSign;
            _constructors[(int)StageObjects.StageObjectType.SnailShell] = StageObjectFactory.CreateSnailShell;
            _constructors[(int)StageObjects.StageObjectType.FadeInOutBox] = StageObjectFactory.CreateFadeInOutBox;
            _constructors[(int)StageObjects.StageObjectType.C4] = StageObjectFactory.CreateC4;
            _constructors[(int)StageObjects.StageObjectType.LaserBeam] = StageObjectFactory.CreateLaserBeam;
            _constructors[(int)StageObjects.StageObjectType.LaserBeamMirror] = StageObjectFactory.CreateLaserBeamMirror;
            _constructors[(int)StageObjects.StageObjectType.ControllableLaserCannon] = StageObjectFactory.CreateControllableLaserCannon;
            _constructors[(int)StageObjects.StageObjectType.LaserBeamSwitch] = StageObjectFactory.CreateLaserBeamSwitch;
            _constructors[(int)StageObjects.StageObjectType.Acid] = StageObjectFactory.CreateAcid;
            _constructors[(int)StageObjects.StageObjectType.Lava] = StageObjectFactory.CreateLava;
            _constructors[(int)StageObjects.StageObjectType.WaterBubble] = StageObjectFactory.CreateWaterBubble;
            _constructors[(int)StageObjects.StageObjectType.LiquidPump] = StageObjectFactory.CreateLiquidPump;
            _constructors[(int)StageObjects.StageObjectType.LiquidPipe] = StageObjectFactory.CreateLiquidPipe;
            _constructors[(int)StageObjects.StageObjectType.LiquidTap] = StageObjectFactory.CreateLiquidTap;
            _constructors[(int)StageObjects.StageObjectType.EvilSnail] = StageObjectFactory.CreateEvilSnail;
            _constructors[(int)StageObjects.StageObjectType.FixedLaserCannon] = StageObjectFactory.CreateFixedLaserCannon;
            _constructors[(int)StageObjects.StageObjectType.DirectionalBox] = StageObjectFactory.CreateDirectionalBox;
            _constructors[(int)StageObjects.StageObjectType.TutorialSign] = StageObjectFactory.CreateTutorialSign;
            _constructors[(int)StageObjects.StageObjectType.DynamiteBoxCounted] = StageObjectFactory.CreateDynamiteBoxCounted;
            _constructors[(int)StageObjects.StageObjectType.Slime] = StageObjectFactory.CreateSlime;
        }

        public static StageObject Create(StageObjects.StageObjectType type)
        {
            return _constructors[(int)type]();
        }

        private static Snail CreateSnail() { return new Snail(); }
        private static StageExit CreateStageExit() { return new StageExit(); }
        private static StageEntrance CreateStageEntrance() { return new StageEntrance(); }
        private static SnailCounter CreateSnailCounter() { return new SnailCounter(); }
        private static Dynamite CreateDynamite() { return new Dynamite();}
        private static Vitamin CreateVitamin() { return new Vitamin(); }
        private static Box CreateBox() { return new Box();}
        private static Copper CreateCopper() { return new Copper();}
        private static CollisionTester CreateCollisionTeste() { return new CollisionTester();}
        private static Explosion CreateExplosion() { return  new Explosion();}
        private static Apple CreateApple() { return  new Apple();}
        private static Trampoline CreateTrampoline() { return  new Trampoline();}
        private static Spikes CreateSpikes() { return new Spikes();}
        private static SnailTriggerSwitch CreateTriggerSwitch() { return new SnailTriggerSwitch();}
        private static TeleportEntrance CreateTeleportEntrance() { return  new TeleportEntrance();}
        private static TeleportExit CreateTeleportExit() { return new TeleportExit();}
		private static PickableObject CreatePickableObject() { return  new PickableObject();}
        private static Prop CreateProp() { return new Prop();}
        private static Fire CreateFire() { return new Fire();}
        private static Salt CreateSalt() { return new Salt();}
        private static DynamiteBox CreateDynamiteBox() { return new DynamiteBox();}
        private static DynamiteBoxTriggered CreateDynamiteBoxTriggered() { return new DynamiteBoxTriggered();}
        private static SnailSacrificeSwitch CreateSnailSacrifice() { return new SnailSacrificeSwitch();}
        private static SnailKing CreateSnailKing() { return new SnailKing();}
        private static PopUpBox CreatePopUpBox() { return new PopUpBox();}
        private static Lamp CreateLamp() { return new Lamp();}
        private static FlameLight CreateFlameLight() { return new FlameLight();}
        private static Water CreateWater() { return new Water();}
        private static Crystal CreateCrystal() { return new Crystal();}
        private static StageProp CreateStageProp() { return new StageProp();}
        private static InformationSign CreateInformationSign() { return new InformationSign();}
        private static SnailShell CreateSnailShell() { return new SnailShell();}
        private static FadeInOutBox CreateFadeInOutBox() { return new FadeInOutBox(); }
        private static C4 CreateC4() { return new C4(); }
        private static LaserBeam CreateLaserBeam() { return new LaserBeam(); }
        private static LaserBeamMirror CreateLaserBeamMirror() { return new LaserBeamMirror(); }
        private static ControllableLaserCannon CreateControllableLaserCannon() { return new ControllableLaserCannon(); }
        private static LaserBeamSwitch CreateLaserBeamSwitch() { return new LaserBeamSwitch(); }
        private static Acid CreateAcid() { return new Acid(); }
        private static Lava CreateLava() { return new Lava(); }
        private static WaterBubble CreateWaterBubble() { return new WaterBubble(); }
        private static LiquidPipe CreateLiquidPipe() { return new LiquidPipe(); }
        private static LiquidPump CreateLiquidPump() { return new LiquidPump(); }
        private static LiquidTap CreateLiquidTap() { return new LiquidTap(); }
        private static EvilSnail CreateEvilSnail() { return new EvilSnail(); }
        private static FixedLaserCannon CreateFixedLaserCannon() { return new FixedLaserCannon(); }
        private static DirectionalBox CreateDirectionalBox() { return new DirectionalBox(); }
        private static TutorialSign CreateTutorialSign() { return new TutorialSign(); }
        private static DynamiteBoxCounted CreateDynamiteBoxCounted() { return new DynamiteBoxCounted(); }
        private static Slime CreateSlime() { return new Slime(); }
    }
}
