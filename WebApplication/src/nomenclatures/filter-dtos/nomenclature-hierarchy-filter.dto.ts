import { Level } from "src/shared/enums/level.enum";
import { NomenclatureHierarchyDto } from "../dtos/nomenclature-hierarchy.dto";
import { NomenclatureFilterDto } from "./nomenclature-filter.dto";

export class NomenclatureHierarchyFilterDto<T extends NomenclatureHierarchyDto<T>> extends NomenclatureFilterDto {
    level: Level;
    excludeLevel: boolean;
    rootId: number;
    root: T;
    parentId: number;
    parent: T;
    isRoot: boolean;
}