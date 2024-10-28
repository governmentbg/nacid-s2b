import { Injectable } from '@angular/core';
import { DistrictDto } from 'src/nomenclatures/dtos/settlements/district.dto';
import { MunicipalityDto } from 'src/nomenclatures/dtos/settlements/municipality.dto';
import { SettlementDto } from 'src/nomenclatures/dtos/settlements/settlement.dto';

@Injectable()
export class SettlementChangeService {

    districtChange(entity: any, district: DistrictDto, districtName: string, municipalityName: string = null, settlementName: string = null) {
        if (district) {
            entity[districtName] = district;
            entity[`${districtName}Id`] = district.id;
        } else {
            entity[districtName] = null;
            entity[`${districtName}Id`] = null;
        }

        if (municipalityName) {
            entity[municipalityName] = null;
            entity[`${municipalityName}Id`] = null;
        }

        if (settlementName) {
            entity[settlementName] = null;
            entity[`${settlementName}Id`] = null;
        }
    }

    municipalityChange(entity: any, municipality: MunicipalityDto, municipalityName: string, districtName: string = null, settlementName: string = null) {
        if (municipality) {
            entity[municipalityName] = municipality;
            entity[`${municipalityName}Id`] = municipality.id;

            if (districtName) {
                entity[districtName] = municipality.district;
                entity[`${districtName}Id`] = municipality.districtId;
            }
        } else {
            entity[municipalityName] = null;
            entity[`${municipalityName}Id`] = null;
        }

        if (settlementName) {
            entity[settlementName] = null;
            entity[`${settlementName}Id`] = null;
        }
    }

    settlementChange(entity: any, settlement: SettlementDto, settlementName: string, districtName: string = null, municipalityName: string = null) {
        if (settlement) {
            if (municipalityName) {
                entity[municipalityName] = settlement.municipality;
                entity[`${municipalityName}Id`] = settlement.municipalityId;
            }

            if (districtName) {
                entity[districtName] = settlement.district;
                entity[`${districtName}Id`] = settlement.districtId;
            }

            entity[settlementName] = settlement;
            entity[`${settlementName}Id`] = settlement.id;
        } else {
            entity[settlementName] = null;
            entity[`${settlementName}Id`] = null;
        }
    }
}
