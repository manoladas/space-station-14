using Content.Shared.Access.Components;
using Content.Shared.SprayPainter.Prototypes;
using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;

namespace Content.Client.Access;

public sealed class IdCardVisualizerSystem : VisualizerSystem<IdCardComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, IdCardComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        string? prototype;
        if (!args.TryGetData(PaintableVisuals.Prototype, out prototype))
        {
            if (!TryComp<MetaDataComponent>(uid, out var metadata)
                || metadata.EntityPrototype == null)
                return;

            prototype = metadata.EntityPrototype.ID;
        }

        if (!ProtoMan.HasIndex(prototype))
            return;

        var paintedCard = Spawn(prototype);
        if (TryComp<SpriteComponent>(paintedCard, out var paintedSprite))
            SpriteSystem.CopySprite((paintedCard, paintedSprite), (uid, args.Sprite));

        QueueDel(paintedCard);
    }
}
