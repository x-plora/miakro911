﻿ALTER TABLE  `buildings` CHANGE  `b_farm`  `b_farm` INT( 10 ) UNSIGNED NULL DEFAULT NULL ;
UPDATE buildings SET b_farm = NULL WHERE b_farm = 0;

ALTER TABLE  `buildings` 
	DROP INDEX  `b_farm`, 
	ADD UNIQUE  `b_farm` (`b_farm`) COMMENT  '';

ALTER TABLE  `buildings` 
	ADD FOREIGN KEY (  `b_farm` ) REFERENCES  `minifarms` (`m_id`) 
		ON DELETE SET NULL 
		ON UPDATE CASCADE;


UPDATE minifarms SET m_lower = NULL WHERE m_lower = 0;

ALTER TABLE `minifarms` 
	ADD FOREIGN KEY (`m_upper`) REFERENCES  `tiers` (`t_id`) 
		ON DELETE SET NULL 
		ON UPDATE CASCADE ;

ALTER TABLE `minifarms` 
	ADD FOREIGN KEY (`m_lower`) REFERENCES  `tiers` (`t_id`) 
		ON DELETE SET NULL 
		ON UPDATE CASCADE;


ALTER TABLE  `logs` 
	ADD FOREIGN KEY (`l_type`) REFERENCES `logtypes` (`l_type`) 
		ON DELETE CASCADE 
		ON UPDATE CASCADE;



UPDATE options SET o_value = '17' WHERE o_name = 'db' AND o_subname = 'version';