CREATE TABLE `sensor`
(
    `ts_in`       datetime NOT NULL,
    `measurement` json     NOT NULL
) ENGINE = InnoDB;

SELECT s.*, TIMEDIFF(ts_meas, ts_in) AS ts_diff
FROM (
         SELECT ts_in,
                STR_TO_DATE(measurement ->> "$.ts_meas", '%d.%m.%Y %H:%i:%s') AS ts_meas,
                measurement ->> "$.id"                                        AS id,
                measurement ->> "$.temp"                                      AS temp,
                measurement ->> "$.battery"                                   AS battery,
                measurement ->> "$.load"                                      AS `load`
         FROM sensor) AS s
