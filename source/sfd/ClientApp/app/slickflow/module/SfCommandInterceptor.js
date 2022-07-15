import CommandInterceptor from "diagram-js/lib/command/CommandInterceptor";
import jshelper from "../../script/jshelper";

class SfCommandInterceptor extends CommandInterceptor {
    constructor(eventBus, modeling) {
        super(eventBus);

        this.postExecuted(["shape.create"], ({ context }) => {
            var businessObject = context.shape.businessObject;

            if (businessObject.guid === undefined) {
                modeling.updateProperties(context.shape, {
                    guid: jshelper.getUUID()
                });
            }
        });

        this.postExecuted(["connection.create"], ({ context }) => {
            var source = context.source;
            var target = context.target;
            var from = source.businessObject.guid;
            var to = target.businessObject.guid;

            var businessObject = context.connection.businessObject;
            if (businessObject.guid === undefined) {
                modeling.updateProperties(context.connection, {
                    guid: jshelper.getUUID(),
                    'sf:from': from,
                    'sf:to': to
                })
            }
        });
    }
}

SfCommandInterceptor.$inject = ["eventBus", "modeling"];

export default {
    __init__: ["sfCommandInterceptor"],
    sfCommandInterceptor: ["type", SfCommandInterceptor]
};

