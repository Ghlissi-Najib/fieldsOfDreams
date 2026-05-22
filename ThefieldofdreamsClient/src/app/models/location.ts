export type LocationType =
  | 'Stadium'
  | 'TouristAttraction'
  | 'Restaurant'
  | 'Hotel'
  | 'SponsorBooth'
  | 'HiddenGem'
  | 'TransportationHub'
  | 'CruisePort';

export interface Location {
  id: string;
  name: string;
  description?: string;
  latitude: number;
  longitude: number;
  address?: string;
  city?: string;
  country?: string;
  type: LocationType;
  imageUrl?: string;
  pointsBonus: number;
  isTourismSpot: boolean;
  proximityRadiusMeters?: number;
}

export interface CreateLocationRequest {
  name: string;
  description?: string;
  latitude: number;
  longitude: number;
  address?: string;
  city?: string;
  country?: string;
  type: LocationType;
  imageUrl?: string;
  pointsBonus: number;
  isTourismSpot: boolean;
  proximityRadiusMeters?: number;
}

export interface UpdateLocationRequest extends CreateLocationRequest {}
