export type CruiseSessionStatus = 'Approaching' | 'InPort' | 'Departed';

export interface CruiseSession {
  id: string;
  shipName: string;
  cruiseLine?: string;
  voyageNumber?: string;
  portLocationId: string;
  arrivalTime: string;
  departureTime?: string;
  estimatedPassengers: number;
  status: CruiseSessionStatus;
  distanceToPortMeters?: number;
  missionsAssigned: boolean;
  createdAt: string;
}

export interface CreateCruiseSessionRequest {
  shipName: string;
  cruiseLine?: string;
  voyageNumber?: string;
  portLocationId: string;
  arrivalTime: string;
  departureTime?: string;
  estimatedPassengers: number;
}

export interface UpdateCruiseSessionRequest {
  status: CruiseSessionStatus;
  departureTime?: string;
  estimatedPassengers?: number;
  missionsAssigned?: boolean;
}
