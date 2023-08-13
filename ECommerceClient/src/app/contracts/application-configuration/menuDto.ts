export class MenuDto {
    name: string;
    actions: ActionDto[];
}

export class ActionDto {
    actionType: string;
    httpType: string;
    definition: string;
    code: string;
}