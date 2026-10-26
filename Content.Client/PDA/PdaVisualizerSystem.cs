using Content.Shared.Light;
using Content.Shared.PDA;
using Content.Shared.SprayPainter.Prototypes;
using Robust.Client.GameObjects;

namespace Content.Client.PDA;

public sealed partial class PdaVisualizerSystem : VisualizerSystem<PdaVisualsComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, PdaVisualsComponent comp, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (args.TryGetData<string>(PdaVisuals.PdaType, out var pdaType))
            SpriteSystem.LayerSetRsiState((uid, args.Sprite), PdaVisualLayers.Base, pdaType);

        if (args.TryGetData<string>(PaintableVisuals.Prototype, out var prototype)
            && ProtoMan.HasIndex(prototype))
        {
            var paintedPda = Spawn(prototype);
            if (TryComp<SpriteComponent>(paintedPda, out var paintedSprite))
            {
                var state = SpriteSystem.LayerGetRsiState((paintedPda, paintedSprite), (int) PdaVisualLayers.Base);
                SpriteSystem.LayerSetRsiState((uid, args.Sprite), PdaVisualLayers.Base, state);
            }

            QueueDel(paintedPda);
        }

        if (args.TryGetData<bool>(UnpoweredFlashlightVisuals.LightOn, out var isFlashlightOn))
            SpriteSystem.LayerSetVisible((uid, args.Sprite), PdaVisualLayers.Flashlight, isFlashlightOn);

        if (args.TryGetData<bool>(PdaVisuals.IdCardInserted, out var isCardInserted))
            SpriteSystem.LayerSetVisible((uid, args.Sprite), PdaVisualLayers.IdLight, isCardInserted);
    }

    public enum PdaVisualLayers : byte
    {
        Base,
        Flashlight,
        IdLight
    }
}
