export interface WeatherCurrent {
    city: string;
    country: string;
    latitude: number;
    longitude: number;
    temperature: number;
    weatherCode: number;
    weatherDescription: string;
    windSpeed: number;
    timestamp: string;
}

export interface WeatherTomorrow {
    city: string;
    country: string;
    latitude: number;
    longitude: number;
    date: string;
    temperatureMin: number;
    temperatureMax: number;
    weatherCode: number;
    weatherDescription: string;
    precipitationProbability: number;
}

export interface WeatherFavoriteLocation {
    id: number;
    cityName: string;
    country: string;
    latitude: number;
    longitude: number;
    createdAt: string;
}

export interface CreateWeatherFavoriteLocationRequest {
    cityName: string;
    country: string;
    latitude: number;
    longitude: number;
}
