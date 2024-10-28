import { Level } from "src/shared/enums/level.enum";
import { NomenclatureDto } from "./nomenclature.dto";

export class NomenclatureHierarchyDto<T extends NomenclatureHierarchyDto<T>> extends NomenclatureDto {
    rootId: number;
    root: T;
    parentId: number;
    parent: T;
    level: Level;
}