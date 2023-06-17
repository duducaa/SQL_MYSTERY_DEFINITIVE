USE sql_mystery;

DELIMITER $

#SÓ PODE ADICINAR NA TB_CONVITE OS PERSONAGENS CONVIDADOS DA TB_PERFIL
CREATE TRIGGER TG_CONVITE_PERSONAGEM
BEFORE INSERT
ON TB_CONVITE
FOR EACH ROW
BEGIN
	DECLARE vPerfil TEXT;
    SET vPerfil = (
		SELECT
			PL.NOME
		FROM
			TB_PERSONAGEM PM
		INNER JOIN
			TB_PERFIL PL
		ON
			PL.ID_PERFIL = PM.ID_PERFIL
		WHERE
			PM.ID_PERSONAGEM = NEW.ID_PERSONAGEM
    );
    
    IF UPPER(vPerfil) != 'CONVIDADO' THEN
		SIGNAL SQLSTATE '45001' SET MESSAGE_TEXT = 'ESSE PERSONAGEM É FUNCIONÁRIO';     
   END IF;
END;

#SÓ PODE ADICINAR NA TB_CONTRATO OS PERSONAGENS FUNCIONÁRIOS DA TB_PERFIL
CREATE TRIGGER TG_CONTRATO_PERSONAGEM
BEFORE INSERT
ON TB_CONTRATO
FOR EACH ROW
BEGIN
	DECLARE vPerfil TEXT;
    SET vPerfil = (
		SELECT
			PL.NOME
		FROM
			TB_PERSONAGEM PM
		INNER JOIN
			TB_PERFIL PL
		ON
			PL.ID_PERFIL = PM.ID_PERFIL
		WHERE
			PM.ID_PERSONAGEM = NEW.ID_PERSONAGEM
    );
    
    IF UPPER(vPerfil) != 'FUNCIONÁRIO' THEN
		SIGNAL SQLSTATE '45002' SET MESSAGE_TEXT = 'ESSE PERSONAGEM É CONVIDADO';     
   END IF;
END;

$ DELIMITER ;