export interface PersonAstronaut {
  personId: number;
  name: string;
  currentRank: string;
  currentDutyTitle: string;
  careerStartDate: string | null;
  careerEndDate: string | null;
}

export interface AstronautDuty {
  id: number;
  personId: number;
  rank: string;
  dutyTitle: string;
  dutyStartDate: string;
  dutyEndDate: string | null;
}

export interface AstronautDutyResponse {
  success: boolean;
  message: string;
  responseCode: number;
  person: PersonAstronaut | null;
  astronautDuties: AstronautDuty[];
}

export interface CreateAstronautDutyRequest {
  name: string;
  rank: string;
  dutyTitle: string;
  dutyStartDate: string;
}

export interface CreateAstronautDutyResponse {
  success: boolean;
  message: string;
  responseCode: number;
  id?: number | null;
}
