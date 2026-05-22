export type PredictionStatus = 'Pending' | 'Completed' | 'Cancelled';

export interface Prediction {
  id: string;
  userId: string;
  matchId: string;
  homeTeamScore?: number;
  awayTeamScore?: number;
  winnerPrediction?: string;
  pointsEarned: number;
  status: PredictionStatus;
  isCorrect: boolean;
  predictedAt: string;
}

export interface CreatePredictionRequest {
  matchId: string;
  homeTeamScore?: number;
  awayTeamScore?: number;
  winnerPrediction?: string;
}

export interface UpdatePredictionRequest {
  homeTeamScore?: number;
  awayTeamScore?: number;
  winnerPrediction?: string;
  status: PredictionStatus;
  isCorrect: boolean;
  pointsEarned: number;
}
