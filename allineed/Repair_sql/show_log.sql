﻿SELECT l_id,l_date,(select l_name from logtypes where l_type=l.l_type) type,l_rabbit,l_address,l_rabbit2,l_address2,l_param FROM `logs` l order by l_id desc;