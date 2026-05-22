export type MatchStatus = 'Scheduled' | 'Live' | 'Completed' | 'Cancelled';

export interface Match {
  id: string;
  title: string;
  homeTeam?: string;
  awayTeam?: string;
  homeTeamScore?: number;
  awayTeamScore?: number;
  matchDateTime: string;
  stadium?: string;
  location?: string;
  tournamentStage?: string;
  status: MatchStatus;
  totalPredictions: number;
  pointsForCorrectPrediction: number;
}

export interface CreateMatchRequest {
  title: string;
  homeTeam?: string;
  awayTeam?: string;
  matchDateTime: string;
  stadium?: string;
  location?: string;
  tournamentStage?: string;
  status?: MatchStatus;
  pointsForCorrectPrediction?: number;
}

export interface UpdateMatchRequest {
  title: string;
  homeTeam?: string;
  awayTeam?: string;
  homeTeamScore?: number;
  awayTeamScore?: number;
  matchDateTime: string;
  stadium?: string;
  location?: string;
  tournamentStage?: string;
  status?: MatchStatus;
  totalPredictions?: number;
  pointsForCorrectPrediction?: number;
}
