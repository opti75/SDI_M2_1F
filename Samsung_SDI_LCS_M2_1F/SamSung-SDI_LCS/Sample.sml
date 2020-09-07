<XCom 2.4>
<S1F1 P Are You There Request
>

<S1F2 S Online Data
  <LIST 2 
    <ASCII 10 MDLN>
    <ASCII 10 SOFTREV>
  >
>

<S1F2 P Online Data
  <LIST 0 >
>

<S1F3 P Selected Equipment Status Request
  <LIST n 
    <UINT2 1 SVID>
  >
>

<S1F4 S Selected Equipment Status Data
  <LIST 1 
    <UINT2 1 >
  >
>

<S1F4 S Selected Equipment Status Data(SVID=21)
  <LIST 1 
    <LIST n 
      <LIST 5 
        <ASCII 16 CarrierID>
        <ASCII 16 VehicleID>
        <ASCII 16 CarrierLoc>
        <ASCII 16 Install>
        <UINT2 1 CarrierType>
      >
    >
  >
>

<S1F4 S Selected Equipment Status Data(SVID=23)
  <LIST 1 
    <LIST n 
      <LIST 3 
        <LIST 2 
          <ASCII 40 CommandID>
          <UINT2 1 Priority>
        >
        <UINT2 1 TransferState>
        <LIST 1 
          <LIST 3 
            <ASCII 40 CarrierID>
            <ASCII 40 SourcePort>
            <ASCII 40 DestPort>
          >
        >
      >
    >
  >
>

<S1F4 S Selected Equipment Status Data(SVID=25)
  <LIST 1 
    <LIST n 
      <LIST 2 
        <ASCII 16 VehicleID>
        <UINT2 1 Vehicle State>
      >
    >
  >
>

<S1F4 S Selected Equipment Status Data(SVID=61)
  <LIST 1 
    <LIST n 
      <LIST 3 
        <ASCII 80 UnitID>
        <UINT4 1 ALID>
        <ASCII 80 ALTX>
      >
    >
  >
>

<S1F11 P Status Variable Namelist Request
  <LIST n 
    <UINT2 1 SVID>
  >
>

<S1F12 S Status Variable Namelist Reply
  <LIST n 
    <LIST 3 
      <UINT2 1 SVID>
      <ASCII 30 SVNAME>
      <ASCII 30 SVUNITS>
    >
  >
>

<S1F13 P Establish Communications Request
  <LIST 0 >
>

<S1F13 S Establish Communications Request
  <LIST 2 
    <ASCII 6 MDLN>
    <ASCII 6 SOFTREV>
  >
>

<S1F14 P Establish Communications Request ACK
  <LIST 2 
    <BINARY 1 COMMACK>
    <LIST 0 >
  >
>

<S1F14 S Establish Communications Request ACK
  <LIST 2 
    <BINARY 1 COMMACK>
    <LIST 2 
      <ASCII 6 MDLN>
      <ASCII 6 SOFTREV>
    >
  >
>

<S1F15 P Request OFF-LINE
>

<S1F16 S OFF-LINE Acknowledge
  <BINARY 1 OFLACK>
>

<S1F17 P Request ON-LINE
>

<S1F18 S ON-LINE Acknowledge
  <BINARY 1 ONLACK>
>

<S2F13 P Equipment Constant Request
  <LIST n 
    <UINT2 1 ECID>
  >
>

<S2F14 S Equipment Constant Data
  <LIST n 
    <ASCII 16 ECV>
  >
>

<S2F15 P New Equipment Constant Send
  <LIST n 
    <LIST 2 
      <UINT2 1 ECID>
      <UINT2 1 ECV>
    >
  >
>

<S2F16 S New Equipment Constant Acknowledge
  <BINARY 1 EAC>
>

<S2F17 S Date and Time Request
>

<S2F18 P Date and Time Data
  <ASCII 16 TIME>
>

<S2F29 P Equipment Constant Namelist Request
  <LIST n 
    <UINT2 1 ECID>
  >
>

<S2F30 S Equipment Constant Namelist 
  <LIST n 
    <LIST 6 
      <UINT2 1 ECID>
      <ASCII 64 ECNAME>
      <UINT2 1 ECMIN>
      <UINT2 1 ECMAX>
      <UINT2 1 ECDEF>
      <ASCII 32 UNITS>
    >
  >
>

<S2F31 P Date and Time Data
  <ASCII 16 TIME>
>

<S2F32 S Date and Time Data Set Acknowledge
  <BINARY 1 TIACK>
>

<S2F33 P Report Data
  <LIST 2 
    <UINT4 1 DATAID>
    <LIST n 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST n 
          <UINT2 1 VID>
        >
      >
    >
  >
>

<S2F34 S Define Report Acknowledge
  <BINARY 1 RDACK>
>

<S2F35 P Link Event Report
  <LIST 2 
    <UINT4 1 DATAID>
    <LIST n 
      <LIST 2 
        <UINT2 1 CEID>
        <LIST n 
          <UINT2 1 PRTID>
        >
      >
    >
  >
>

<S2F36 S Link Event Report Acknowledge
  <BINARY 1 LRACK>
>

<S2F37 P Enable/Disable Event Report
  <LIST 2 
    <BOOL 1 CEED>
    <LIST n 
      <UINT2 1 CEID>
    >
  >
>

<S2F38 S Enable/Disable Event Report Acknowledge
  <BINARY 1 ERACK>
>

<S2F41 P Host Command Send (PAUSE, RESUME)
  <LIST 2 
    <ASCII 10 RCMD>
    <LIST 0 >
  >
>

<S2F41 P Host Command Send (CANCEL, ABORT)
  <LIST 2 
    <ASCII 10 RCMD>
    <LIST 1 
      <LIST 2 
        <ASCII 32 COMMANDID>
        <ASCII 32 CommandId>
      >
    >
  >
>

<S2F41 P Host Command Send (PRIORITYUPDATE)
  <LIST 2 
    <ASCII 32 RCMD>
    <LIST 2 
      <LIST 2 
        <ASCII 32 COMMANDID>
        <ASCII 32 CommandId>
      >
      <LIST 2 
        <ASCII 32 PRIORITY>
        <UINT2 1 Priority>
      >
    >
  >
>

<S2F41 P Host Command Send (TRANSFERUPDATE)
  <LIST 2 
    <ASCII 32 RCMD>
    <LIST 2 
      <LIST 2 
        <ASCII 64 CARRIERID>
        <ASCII 32 CPVAL>
      >
      <LIST 2 
        <ASCII 32 DEST>
        <ASCII 32 CPVAL>
      >
    >
  >
>

<S2F42 S Host Command Ack (HCACK != 3)
  <LIST 2 
    <BINARY 1 HCACK>
    <LIST 0 >
  >
>

<S2F42 S Host Command Ack (HCACK = 3)
  <LIST 2 
    <BINARY 1 HCACK>
    <LIST n 
      <LIST 2 
        <ASCII 64 CPNAME>
        <BINARY 1 CPACK>
      >
    >
  >
>

<S2F49 P Enhanced Remote Command
  <LIST 4 
    <UINT4 1 DATAID>
    <ASCII 0 OBJSPEC>
    <ASCII 40 RCMD (TRANSFER)>
    <LIST 2 
      <LIST 2 
        <ASCII 40 CPNAME (COMMANDINFO)>
        <LIST 2 
          <LIST 2 
            <ASCII 40 CPNAME (COMMANDID)>
            <ASCII 64 CPVAL[0~64] (111111)>
          >
          <LIST 2 
            <ASCII 40 CPNAME (PRIORITY)>
            <UINT2 1 CPVAL>
          >
        >
      >
      <LIST 2 
        <ASCII 40 CPNAME (TRANSFERINFO)>
        <LIST 8 
          <LIST 2 
            <ASCII 40 CARRIERID>
            <ASCII 40 CARRIER1>
          >
          <LIST 2 
            <ASCII 40 SOURCEPORT>
            <ASCII 64 PORTXX>
          >
          <LIST 2 
            <ASCII 40 DESTPORT>
            <ASCII 64 PORTYY>
          >
          <LIST 2 
            <ASCII 40 CARRIERTYPE>
            <UINT2 1 CPVAL >
          >
          <LIST 2 
            <ASCII 40 PROCESSID>
            <ASCII 64 CPVAL (PRO01)>
          >
          <LIST 2 
            <ASCII 40 BATCHID>
            <ASCII 64 CPVAL (BAT01)>
          >
          <LIST 2 
            <ASCII 40 LOTID>
            <ASCII 64 CPVAL (LOT01)>
          >
          <LIST 2 
            <ASCII 40 >
            <LIST n 
              <ASCII 40 CARRIERIDn>
            >
          >
        >
      >
    >
  >
>

<S2F50 S Enhanced Remote Command Ack (HCACK != 3)
  <LIST 2 
    <BINARY 1 HCACK>
    <LIST 0 >
  >
>

<S2F50 S Enhanced Remote Command Ack (HCACK = 3)
  <LIST 2 
    <BINARY 1 >
    <LIST n 
      <LIST 2 
        <ASCII 64 CPNAME>
        <BINARY 1 CPACK>
      >
    >
  >
>

<S5F1 P Alarm Report Send
  <LIST 3 
    <BINARY 1 >
    <UINT4 1 >
    <ASCII 80 >
  >
>

<S5F2 S Alarm Report Send Ack
  <BINARY 1 ACKC5>
>

<S5F3 P Enable/Disable Alarm Send
  <LIST 2 
    <BINARY 1 ALED>
    <UINT4 1 ALID>
  >
>

<S5F4 S Enable/Disable Alarm Send Ack
  <BINARY 1 ACKC5>
>

<S5F5 P List Alarm Request
  <LIST n 
    <UINT4 1 ALID>
  >
>

<S5F6 S List Alarm Data
  <LIST n 
    <LIST 3 
      <BINARY 1 ALCD>
      <UINT4 1 ALID>
      <ASCII 80 ALTX>
    >
  >
>

<S6F11 N Event Report Send (RPTID=1, EqpName)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 1 
          <ASCII 30 EqpName>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=2, VehicleInfo)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 CommandID>
          <LIST 2 
            <ASCII 16 VehicleID>
            <UINT2 1 Vehicle State>
          >
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=3, Transfer Complete Info)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 CommandID>
          <LIST 1 
            <LIST 2 
              <LIST 3 
                <ASCII 16 CarrierID>
                <ASCII 16 SourcePort>
                <ASCII 16 DestPort>
              >
              <ASCII 16 CarrierLoc>
            >
          >
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=4)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 1 
          <ASCII 30 CommandID>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=5)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 3 
          <LIST 2 
            <ASCII 30 CommandID>
            <UINT2 1 Priority>
          >
          <LIST 1 
            <LIST 2 
              <LIST 3 
                <ASCII 16 CarrierID>
                <ASCII 16 SourcePort>
                <ASCII 16 DestPort>
              >
              <ASCII 16 CarrierLoc>
            >
          >
          <UINT2 1 ResultCode>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=6)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 5 
          <ASCII 30 VehicleID>
          <ASCII 30 CarrierID>
          <ASCII 30 CarrierLoc>
          <ASCII 30 CommandID>
          <UINT2 1 CarrierType>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=8)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 7 
          <ASCII 30 CommandID>
          <ASCII 30 CommandType>
          <ASCII 30 CarrierID>
          <ASCII 30 SourcePort>
          <ASCII 30 DestPort>
          <UINT2 1 Priority>
          <UINT2 1 CarrierType>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=9)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 VehicleID>
          <ASCII 30 TransferPort>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=10)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 3 
          <ASCII 30 VehicleID>
          <ASCII 30 TransferPort>
          <ASCII 30 CarrierID>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=11)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 VehicleID>
          <ASCII 30 CommandID>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=12)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 1 
          <ASCII 30 VehicleID>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=13)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 3 
          <ASCII 30 UnitID>
          <UINT4 1 AlramID>
          <ASCII 30 AlramText>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=14)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 CommandID>
          <UINT2 1 Priority>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=15)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 VehicleID>
          <ASCII 30 VehicleCurrentPosition>
        >
      >
    >
  >
>

<S6F11 N Event Report Send (RPTID=16)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 VehicleID>
          <ASCII 30 VehicleState>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=1, EqpName)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 1 
          <ASCII 30 EqpName>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=2, VehicleInfo)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 CommandID>
          <LIST 2 
            <ASCII 16 VehicleID>
            <UINT2 1 Vehicle State>
          >
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=3, Transfer Complete Info)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 CommandID>
          <LIST 1 
            <LIST 2 
              <LIST 3 
                <ASCII 16 CarrierID>
                <ASCII 16 SourcePort>
                <ASCII 16 DestPort>
              >
              <ASCII 16 CarrierLoc>
            >
          >
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=4)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 1 
          <ASCII 30 CommandID>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=5)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 3 
          <LIST 2 
            <ASCII 30 CommandID>
            <UINT2 1 Priority>
          >
          <LIST 1 
            <LIST 2 
              <LIST 3 
                <ASCII 16 CarrierID>
                <ASCII 16 SourcePort>
                <ASCII 16 DestPort>
              >
              <ASCII 16 CarrierLoc>
            >
          >
          <UINT2 1 ResultCode>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=6)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 5 
          <ASCII 30 VehicleID>
          <ASCII 30 CarrierID>
          <ASCII 30 CarrierLoc>
          <ASCII 30 CommandID>
          <UINT2 1 CarrierType>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=8)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 7 
          <ASCII 30 CommandID>
          <ASCII 30 CommandType>
          <ASCII 30 CarrierID>
          <ASCII 30 SourcePort>
          <ASCII 30 DestPort>
          <UINT2 1 Priority>
          <UINT2 1 CarrierType>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=9)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 VehicleID>
          <ASCII 30 TransferPort>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=10)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 3 
          <ASCII 30 VehicleID>
          <ASCII 30 TransferPort>
          <ASCII 30 CarrierID>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=11)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 VehicleID>
          <ASCII 30 CommandID>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=12)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 1 
          <ASCII 30 VehicleID>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=13)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 3 
          <ASCII 30 UnitID>
          <UINT4 1 AlramID>
          <ASCII 30 AlramText>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=14)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 CommandID>
          <UINT2 1 Priority>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=15)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 VehicleID>
          <ASCII 30 VehicleCurrentPosition>
        >
      >
    >
  >
>

<S6F16 N Event Report Send (RPTID=16)
  <LIST 3 
    <UINT4 1 DATAID>
    <UINT2 1 CEID>
    <LIST 1 
      <LIST 2 
        <UINT2 1 RPTID>
        <LIST 2 
          <ASCII 30 VehicleID>
          <ASCII 30 VehicleState>
        >
      >
    >
  >
>

<S6F12 S Event Report Ack
  <BINARY 1 ACKC6>
>

<S6F15 P Event Report Request
  <UINT2 1 CEID>
>

<S6F19 P Individual Report Request
  <UINT2 1 RPTID>
>

<S6F20 N Event Report Send (RPTID=2, VehicleInfo)
  <LIST n 
    <ASCII 30 EqpName>
  >
>

<S6F20 N Event Report Send (RPTID=2, VehicleInfo)
  <LIST n 
    <LIST 2 
      <ASCII 30 CommandID>
      <LIST 2 
        <ASCII 16 VehicleID>
        <UINT2 1 Vehicle State>
      >
    >
  >
>

<S6F20 N Event Report Send (RPTID=3, Transfer Complete Info)
  <LIST n 
    <LIST 2 
      <ASCII 30 CommandID>
      <LIST 1 
        <LIST 2 
          <LIST 3 
            <ASCII 16 CarrierID>
            <ASCII 16 SourcePort>
            <ASCII 16 DestPort>
          >
          <ASCII 16 CarrierLoc>
        >
      >
    >
  >
>

<S6F20 N Event Report Send (RPTID=4)
  <LIST n 
    <ASCII 30 CommandID>
  >
>

<S9F1 N Unrecognized Device ID
>

<S9F3 N Unrecognized Stream Type
>

<S9F5 N Unrecognized Function Type
>

<S9F7 N Illegal Data
>

<S9F9 N Transaction Timer time-out
>

<S9F11 N Data Too Long
>

