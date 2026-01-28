using System.Text.Json.Serialization;

namespace AutoMechanic.CarAPI.Models
{
    public class CarAPIVINSpecs
    {
        [JsonPropertyName("active_safety_system_note")]
        public string? ActiveSafetySystemNote { get; set; }

        [JsonPropertyName("adaptive_cruise_control_acc")]
        public string? AdaptiveCruiseControlAcc { get; set; }

        [JsonPropertyName("adaptive_driving_beam_adb")]
        public string? AdaptiveDrivingBeamAdb { get; set; }

        [JsonPropertyName("anti_lock_braking_system_abs")]
        public string? AntiLockBrakingSystemAbs { get; set; }

        [JsonPropertyName("auto_reverse_system_for_windows_and_sunroofs")]
        public string? AutoReverseSystemForWindowsAndSunroofs { get; set; }

        [JsonPropertyName("automatic_crash_notification_acn_advanced_automatic_crash_notification_aacn")]
        public string? AutomaticCrashNotificationAcnAdvancedAutomaticCrashNotificationAacn { get; set; }

        [JsonPropertyName("automatic_pedestrian_alerting_sound_for_hybrid_and_ev_only")]
        public string? AutomaticPedestrianAlertingSoundForHybridAndEvOnly { get; set; }

        [JsonPropertyName("axle_configuration")]
        public string? AxleConfiguration { get; set; }

        [JsonPropertyName("axles")]
        public string? Axles { get; set; }

        [JsonPropertyName("backup_camera")]
        public string? BackupCamera { get; set; }

        [JsonPropertyName("base_price")]
        public string? BasePrice { get; set; }

        [JsonPropertyName("battery_current_amps_from")]
        public string? BatteryCurrentAmpsFrom { get; set; }

        [JsonPropertyName("battery_current_amps_to")]
        public string? BatteryCurrentAmpsTo { get; set; }

        [JsonPropertyName("battery_energy_kwh_from")]
        public string? BatteryEnergyKwhFrom { get; set; }

        [JsonPropertyName("battery_energy_kwh_to")]
        public string? BatteryEnergyKwhTo { get; set; }

        [JsonPropertyName("battery_type")]
        public string? BatteryType { get; set; }

        [JsonPropertyName("battery_voltage_volts_from")]
        public string? BatteryVoltageVoltsFrom { get; set; }

        [JsonPropertyName("battery_voltage_volts_to")]
        public string? BatteryVoltageVoltsTo { get; set; }

        [JsonPropertyName("bed_length_inches")]
        public string? BedLengthInches { get; set; }

        [JsonPropertyName("bed_type")]
        public string? BedType { get; set; }

        [JsonPropertyName("blind_spot_intervention_bsi")]
        public string? BlindSpotInterventionBsi { get; set; }

        [JsonPropertyName("blind_spot_warning_bsw")]
        public string? BlindSpotWarningBsw { get; set; }

        [JsonPropertyName("body_class")]
        public string? BodyClass { get; set; }

        [JsonPropertyName("brake_system_description")]
        public string? BrakeSystemDescription { get; set; }

        [JsonPropertyName("brake_system_type")]
        public string? BrakeSystemType { get; set; }

        [JsonPropertyName("bus_floor_configuration_type")]
        public string? BusFloorConfigurationType { get; set; }

        [JsonPropertyName("bus_length_feet")]
        public string? BusLengthFeet { get; set; }

        [JsonPropertyName("bus_type")]
        public string? BusType { get; set; }

        [JsonPropertyName("cab_type")]
        public string? CabType { get; set; }

        [JsonPropertyName("charger_level")]
        public string? ChargerLevel { get; set; }

        [JsonPropertyName("charger_power_kw")]
        public string? ChargerPowerKw { get; set; }

        [JsonPropertyName("combined_braking_system_cbs")]
        public string? CombinedBrakingSystemCbs { get; set; }

        [JsonPropertyName("cooling_type")]
        public string? CoolingType { get; set; }

        [JsonPropertyName("crash_imminent_braking_cib")]
        public string? CrashImminentBrakingCib { get; set; }

        [JsonPropertyName("curb_weight_pounds")]
        public string? CurbWeightPounds { get; set; }

        [JsonPropertyName("curtain_air_bag_locations")]
        public string? CurtainAirBagLocations { get; set; }

        [JsonPropertyName("custom_motorcycle_type")]
        public string? CustomMotorcycleType { get; set; }

        [JsonPropertyName("daytime_running_light_drl")]
        public string? DaytimeRunningLightDrl { get; set; }

        [JsonPropertyName("destination_market")]
        public string? DestinationMarket { get; set; }

        [JsonPropertyName("displacement_cc")]
        public string? DisplacementCc { get; set; }

        [JsonPropertyName("displacement_ci")]
        public string? DisplacementCi { get; set; }

        [JsonPropertyName("displacement_l")]
        public string? DisplacementL { get; set; }

        [JsonPropertyName("doors")]
        public string? Doors { get; set; }

        [JsonPropertyName("drive_type")]
        public string? DriveType { get; set; }

        [JsonPropertyName("dynamic_brake_support_dbs")]
        public string? DynamicBrakeSupportDbs { get; set; }

        [JsonPropertyName("electrification_level")]
        public string? ElectrificationLevel { get; set; }

        [JsonPropertyName("electronic_stability_control_esc")]
        public string? ElectronicStabilityControlEsc { get; set; }

        [JsonPropertyName("engine_brake_hp_from")]
        public string? EngineBrakeHpFrom { get; set; }

        [JsonPropertyName("engine_brake_hp_to")]
        public string? EngineBrakeHpTo { get; set; }

        [JsonPropertyName("engine_configuration")]
        public string? EngineConfiguration { get; set; }

        [JsonPropertyName("engine_manufacturer")]
        public string? EngineManufacturer { get; set; }

        [JsonPropertyName("engine_model")]
        public string? EngineModel { get; set; }

        [JsonPropertyName("engine_number_of_cylinders")]
        public string? EngineNumberOfCylinders { get; set; }

        [JsonPropertyName("engine_power_kw")]
        public string? EnginePowerKw { get; set; }

        [JsonPropertyName("engine_stroke_cycles")]
        public string? EngineStrokeCycles { get; set; }

        [JsonPropertyName("entertainment_system")]
        public string? EntertainmentSystem { get; set; }

        [JsonPropertyName("ev_drive_unit")]
        public string? EvDriveUnit { get; set; }

        [JsonPropertyName("event_data_recorder_edr")]
        public string? EventDataRecorderEdr { get; set; }

        [JsonPropertyName("forward_collision_warning_fcw")]
        public string? ForwardCollisionWarningFcw { get; set; }

        [JsonPropertyName("front_air_bag_locations")]
        public string? FrontAirBagLocations { get; set; }

        [JsonPropertyName("fuel_delivery_fuel_injection_type")]
        public string? FuelDeliveryFuelInjectionType { get; set; }

        [JsonPropertyName("fuel_tank_material")]
        public string? FuelTankMaterial { get; set; }

        [JsonPropertyName("fuel_tank_type")]
        public string? FuelTankType { get; set; }

        [JsonPropertyName("fuel_type_primary")]
        public string? FuelTypePrimary { get; set; }

        [JsonPropertyName("fuel_type_secondary")]
        public string? FuelTypeSecondary { get; set; }

        [JsonPropertyName("gross_combination_weight_rating_from")]
        public string? GrossCombinationWeightRatingFrom { get; set; }

        [JsonPropertyName("gross_combination_weight_rating_to")]
        public string? GrossCombinationWeightRatingTo { get; set; }

        [JsonPropertyName("gross_vehicle_weight_rating_from")]
        public string? GrossVehicleWeightRatingFrom { get; set; }

        [JsonPropertyName("gross_vehicle_weight_rating_to")]
        public string? GrossVehicleWeightRatingTo { get; set; }

        [JsonPropertyName("headlamp_light_source")]
        public string? HeadlampLightSource { get; set; }

        [JsonPropertyName("keyless_ignition")]
        public string? KeylessIgnition { get; set; }

        [JsonPropertyName("knee_air_bag_locations")]
        public string? KneeAirBagLocations { get; set; }

        [JsonPropertyName("lane_centering_assistance")]
        public string? LaneCenteringAssistance { get; set; }

        [JsonPropertyName("lane_departure_warning_ldw")]
        public string? LaneDepartureWarningLdw { get; set; }

        [JsonPropertyName("lane_keeping_assistance_lka")]
        public string? LaneKeepingAssistanceLka { get; set; }

        [JsonPropertyName("manufacturer_name")]
        public string? ManufacturerName { get; set; }

        [JsonPropertyName("motorcycle_chassis_type")]
        public string? MotorcycleChassisType { get; set; }

        [JsonPropertyName("motorcycle_suspension_type")]
        public string? MotorcycleSuspensionType { get; set; }

        [JsonPropertyName("non_land_use")]
        public string? NonLandUse { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("number_of_battery_cells_per_module")]
        public string? NumberOfBatteryCellsPerModule { get; set; }

        [JsonPropertyName("number_of_battery_modules_per_pack")]
        public string? NumberOfBatteryModulesPerPack { get; set; }

        [JsonPropertyName("number_of_battery_packs_per_vehicle")]
        public string? NumberOfBatteryPacksPerVehicle { get; set; }

        [JsonPropertyName("number_of_seat_rows")]
        public string? NumberOfSeatRows { get; set; }

        [JsonPropertyName("number_of_seats")]
        public string? NumberOfSeats { get; set; }

        [JsonPropertyName("number_of_wheels")]
        public string? NumberOfWheels { get; set; }

        [JsonPropertyName("other_battery_info")]
        public string? OtherBatteryInfo { get; set; }

        [JsonPropertyName("other_bus_info")]
        public string? OtherBusInfo { get; set; }

        [JsonPropertyName("other_engine_info")]
        public string? OtherEngineInfo { get; set; }

        [JsonPropertyName("other_motorcycle_info")]
        public string? OtherMotorcycleInfo { get; set; }

        [JsonPropertyName("other_restraint_system_info")]
        public string? OtherRestraintSystemInfo { get; set; }

        [JsonPropertyName("other_trailer_info")]
        public string? OtherTrailerInfo { get; set; }

        [JsonPropertyName("parking_assist")]
        public string? ParkingAssist { get; set; }

        [JsonPropertyName("pedestrian_automatic_emergency_braking_paeb")]
        public string? PedestrianAutomaticEmergencyBrakingPaeb { get; set; }

        [JsonPropertyName("plant_city")]
        public string? PlantCity { get; set; }

        [JsonPropertyName("plant_company_name")]
        public string? PlantCompanyName { get; set; }

        [JsonPropertyName("plant_country")]
        public string? PlantCountry { get; set; }

        [JsonPropertyName("plant_state")]
        public string? PlantState { get; set; }

        [JsonPropertyName("possible_values")]
        public string? PossibleValues { get; set; }

        [JsonPropertyName("pretensioner")]
        public string? Pretensioner { get; set; }

        [JsonPropertyName("rear_automatic_emergency_braking")]
        public string? RearAutomaticEmergencyBraking { get; set; }

        [JsonPropertyName("rear_cross_traffic_alert")]
        public string? RearCrossTrafficAlert { get; set; }

        [JsonPropertyName("sae_automation_level_from")]
        public string? SaeAutomationLevelFrom { get; set; }

        [JsonPropertyName("sae_automation_level_to")]
        public string? SaeAutomationLevelTo { get; set; }

        [JsonPropertyName("seat_belt_type")]
        public string? SeatBeltType { get; set; }

        [JsonPropertyName("seat_cushion_air_bag_locations")]
        public string? SeatCushionAirBagLocations { get; set; }

        [JsonPropertyName("semiautomatic_headlamp_beam_switching")]
        public string? SemiautomaticHeadlampBeamSwitching { get; set; }

        [JsonPropertyName("series")]
        public string? Series { get; set; }

        [JsonPropertyName("series2")]
        public string? Series2 { get; set; }

        [JsonPropertyName("side_air_bag_locations")]
        public string? SideAirBagLocations { get; set; }

        [JsonPropertyName("steering_location")]
        public string? SteeringLocation { get; set; }

        [JsonPropertyName("suggested_vin")]
        public string? SuggestedVin { get; set; }

        [JsonPropertyName("tire_pressure_monitoring_system_tpms_type")]
        public string? TirePressureMonitoringSystemTpmsType { get; set; }

        [JsonPropertyName("top_speed_mph")]
        public string? TopSpeedMph { get; set; }

        [JsonPropertyName("track_width_inches")]
        public string? TrackWidthInches { get; set; }

        [JsonPropertyName("traction_control")]
        public string? TractionControl { get; set; }

        [JsonPropertyName("trailer_body_type")]
        public string? TrailerBodyType { get; set; }

        [JsonPropertyName("trailer_length_feet")]
        public string? TrailerLengthFeet { get; set; }

        [JsonPropertyName("trailer_type_connection")]
        public string? TrailerTypeConnection { get; set; }

        [JsonPropertyName("transmission_speeds")]
        public string? TransmissionSpeeds { get; set; }

        [JsonPropertyName("transmission_style")]
        public string? TransmissionStyle { get; set; }

        [JsonPropertyName("trim")]
        public string? Trim { get; set; }

        [JsonPropertyName("trim2")]
        public string? Trim2 { get; set; }

        [JsonPropertyName("turbo")]
        public string? Turbo { get; set; }

        [JsonPropertyName("valve_train_design")]
        public string? ValveTrainDesign { get; set; }

        [JsonPropertyName("vehicle_descriptor")]
        public string? VehicleDescriptor { get; set; }

        [JsonPropertyName("vehicle_type")]
        public string? VehicleType { get; set; }

        [JsonPropertyName("wheel_base_inches_from")]
        public string? WheelBaseInchesFrom { get; set; }

        [JsonPropertyName("wheel_base_inches_to")]
        public string? WheelBaseInchesTo { get; set; }

        [JsonPropertyName("wheel_base_type")]
        public string? WheelBaseType { get; set; }

        [JsonPropertyName("wheel_size_front_inches")]
        public string? WheelSizeFrontInches { get; set; }

        [JsonPropertyName("wheel_size_rear_inches")]
        public string? WheelSizeRearInches { get; set; }

        [JsonPropertyName("wheelie_mitigation")]
        public string? WheelieMitigation { get; set; }

        [JsonPropertyName("windows")]
        public string? Windows { get; set; }
    }
}
