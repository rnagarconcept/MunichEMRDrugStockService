-- Create Table ------------------
CREATE SEQUENCE PHARMACY_DRUG_SYNC_STG_SEQ  START WITH 1 INCREMENT BY 1 MINVALUE 1 CACHE 20 NOCYCLE NOORDER;

CREATE TABLE HOSPITAL.PHARMACY_DRUG_SYNC_STG
(
  ID                   NUMBER NOT NULL, 
  DRUG_CODE		       NVARCHAR2(255), 
  DRUG_QTY             NUMBER(10,2),
  CREATED_ON           DATE DEFAULT SYSDATE, 
  UPDATED_ON           DATE 
  )
TABLESPACE USERS
PCTUSED    0
PCTFREE    10
INITRANS   1
MAXTRANS   255
STORAGE    (
            PCTINCREASE      0
            BUFFER_POOL      DEFAULT
           )
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
MONITORING;

CREATE UNIQUE INDEX PHARMACY_DRUG_SYNC_STG_PK ON PHARMACY_DRUG_SYNC_STG
(ID)
LOGGING
TABLESPACE USERS
PCTFREE    10
INITRANS   2
MAXTRANS   255
STORAGE    (
            PCTINCREASE      0
            BUFFER_POOL      DEFAULT
           )
NOPARALLEL;

CREATE OR REPLACE TRIGGER PHARMACY_DRUG_SYNC_STG 
BEFORE INSERT
ON PHARMACY_DRUG_SYNC_STG 
REFERENCING NEW AS New OLD AS Old
FOR EACH ROW
DECLARE
tmpVar NUMBER;

BEGIN
   tmpVar := 0;

   SELECT PHARMACY_DRUG_SYNC_STG_SEQ.NEXTVAL INTO tmpVar FROM dual;
   :NEW.ID := tmpVar;

   EXCEPTION
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RAISE;
END ;

ALTER TABLE PHARMACY_DRUG_SYNC_STG ADD (
  CONSTRAINT PHARMACY_DRUG_SYNC_STG_PK
 PRIMARY KEY
 (ID));
 



------------ Create Package ---- 

CREATE OR REPLACE PACKAGE HOSPITAL.PHARMACY_DRUG_SYNC_INTEGRATION AS
  TYPE t_varchar2_tab IS TABLE OF VARCHAR2(1000) INDEX BY PLS_INTEGER;
  TYPE t_number_tab    IS TABLE OF NUMBER(10,2)  INDEX BY PLS_INTEGER;

  PROCEDURE SAVE_UPDATE_DRUG_STOCK(p_drugcodes IN t_varchar2_tab,p_quantities IN t_number_tab);
END PHARMACY_DRUG_SYNC_INTEGRATION;

-- package body (procedure implementation)
CREATE OR REPLACE PACKAGE BODY PHARMACY_DRUG_SYNC_INTEGRATION AS

  PROCEDURE SAVE_UPDATE_DRUG_STOCK(p_drugcodes IN t_varchar2_tab,
                           p_quantities IN t_number_tab) IS
  BEGIN
    IF p_drugcodes.COUNT != p_quantities.COUNT THEN
      RAISE_APPLICATION_ERROR(-20001, 'Arrays must have the same length');
    END IF;

    -- simple loop with MERGE for each element (works reliably)
    FOR i IN 1 .. p_drugcodes.COUNT LOOP
      MERGE INTO PHARMACY_DRUG_SYNC_STG t
      USING (SELECT p_drugcodes(i) AS drugcode, p_quantities(i) AS quantity FROM dual) s
      ON (t.DRUG_CODE = s.drugcode)
      WHEN MATCHED THEN
        UPDATE SET t.DRUG_QTY = s.quantity, t.UPDATED_ON = SYSDATE
      WHEN NOT MATCHED THEN
        INSERT (DRUG_CODE, DRUG_QTY, CREATED_ON)
        VALUES (s.drugcode, s.quantity, SYSDATE);
    END LOOP;
  END SAVE_UPDATE_DRUG_STOCK;

END PHARMACY_DRUG_SYNC_INTEGRATION;
/

----------------------------------------------------------------------------------------------------------