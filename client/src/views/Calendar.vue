<template>
    <v-row class="fill-height">
        <v-col>
            <br/>
            <v-sheet class="mx-auto" width="900">
                <v-toolbar flat>
                    <v-dialog v-model="addDialog"
                              width="500">
                        <template v-slot:activator="{ on, attrs }">
                            <v-btn color="red lighten-2"
                                   dark
                                   v-bind="attrs"
                                   v-on="on">
                                Add Event
                            </v-btn>
                        </template>

                        <v-card>
                            <v-card-title class="text-h5 grey lighten-2">
                                Create Event
                            </v-card-title>

                            

                            <v-divider></v-divider>
                            <div class="mx-10">
                                <v-text-field v-model="addEventName" label="Enter Event Name"></v-text-field>
                                <v-menu ref="menu1"
                                        v-model="menu1"
                                        :close-on-content-click="false"
                                        transition="scale-transition"
                                        offset-y
                                        max-width="290px"
                                        min-width="auto">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-text-field v-model="modifystart"
                                                      label="Start Date"
                                                      persistent-hint
                                                      prepend-icon="mdi-calendar"
                                                      v-bind="attrs"
                                                      @blur="modifystart = parseDate(modstartdateFormatted)"
                                                      v-on="on"></v-text-field>
                                    </template>
                                    <v-date-picker v-model="modifystart"
                                                   no-title
                                                   @input="menu1 = false"></v-date-picker>
                                </v-menu>
                                <br />
                                <v-menu ref="menu2"
                                        v-model="menu2"
                                        :close-on-content-click="false"
                                        transition="scale-transition"
                                        offset-y
                                        max-width="290px"
                                        min-width="auto">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-text-field v-model="modifyend"
                                                      label="End Date"
                                                      persistent-hint
                                                      prepend-icon="mdi-calendar"
                                                      v-bind="attrs"
                                                      @blur="modifyend = parseDate(modstartdateFormatted)"
                                                      v-on="on"></v-text-field>
                                    </template>
                                    <v-date-picker v-model="modifyend"
                                                   no-title
                                                   @input="menu2 = false"></v-date-picker>
                                </v-menu>
                                <v-menu ref="menu"
                                        v-model="menu3"
                                        :close-on-content-click="false"
                                        :nudge-right="40"
                                        :return-value.sync="time"
                                        transition="scale-transition"
                                        offset-y
                                        max-width="290px"
                                        min-width="290px">
                                    <template v-slot:activator="{ on, attrs }">
                                        <v-text-field v-model="time"
                                                      label="Enter Time of Event"
                                                      prepend-icon="mdi-clock-time-four-outline"
                                                      readonly
                                                      v-bind="attrs"
                                                      v-on="on"></v-text-field>
                                    </template>
                                    <v-time-picker v-if="menu3"
                                                   v-model="time"
                                                   full-width
                                                   use-seconds
                                                   @click:second="$refs.menu.save(time)"></v-time-picker>
                                </v-menu>
                                <v-checkbox v-model="checkbox"
                                            label="Recurring Event"></v-checkbox>
                                <v-spacer></v-spacer>
                                <v-btn color="primary"
                                       text
                                       @click="createEvent()">
                                    Create
                                </v-btn>
                            </div>
                        </v-card>
                    </v-dialog>
                    <v-btn fab
                           text
                           small
                           color="grey darken-2"
                           @click="prev">
                        <v-icon small>
                            mdi-chevron-left
                        </v-icon>
                    </v-btn>
                    <v-btn fab
                           text
                           small
                           color="grey darken-2"
                           @click="next">
                        <v-icon small>
                            mdi-chevron-right
                        </v-icon>
                    </v-btn>
                    <v-toolbar-title v-if="$refs.calendar">
                        {{ $refs.calendar.title }}
                    </v-toolbar-title>
                    <v-spacer></v-spacer>
                    <v-menu bottom
                            right>
                        <template v-slot:activator="{ on, attrs }">
                            <v-btn outlined
                                   color="grey darken-2"
                                   v-bind="attrs"
                                   v-on="on">
                                <span>{{ typeToLabel[type] }}</span>
                                <v-icon right>
                                    mdi-menu-down
                                </v-icon>
                            </v-btn>
                        </template>
                        <v-list>
                            <v-list-item @click="type = 'day'">
                                <v-list-item-title>Day</v-list-item-title>
                            </v-list-item>
                            <v-list-item @click="type = 'week'">
                                <v-list-item-title>Week</v-list-item-title>
                            </v-list-item>
                            <v-list-item @click="type = 'month'">
                                <v-list-item-title>Month</v-list-item-title>
                            </v-list-item>
                            <v-list-item @click="type = '4day'">
                                <v-list-item-title>4 days</v-list-item-title>
                            </v-list-item>
                        </v-list>
                    </v-menu>
                </v-toolbar>
            </v-sheet>
            <v-sheet class="mx-auto" width="900" height="590">
                <v-calendar ref="calendar"
                            v-model="focus"
                            color="primary"
                            :events="events2"
                            :event-color="getEventColor"
                            :type="type"
                            @click:event="showEvent"
                            @click:more="viewDay()"
                            @click:date="viewDay"
                            @change="updateRange"></v-calendar>
                
                <v-dialog v-model="selectedOpen"
                          width="600px"
                          :close-on-content-click="false"
                          :activator="selectedElement">
                        
                    <v-card color=""
                            min-width="350px"
                            flat>
                        <v-toolbar :color="selectedEvent.color"
                                   dark>

                            <v-toolbar-title v-html="selectedEvent.name"></v-toolbar-title>

                            <v-spacer></v-spacer>
                            <v-btn @click="deleteEvent(selectedEvent.eventId)" class="mr-10" icon>
                                Delete
                            </v-btn>
                            <v-btn @click="modifyEvent(selectedEvent.eventId)" class="mr-4" icon>
                                Update
                            </v-btn>


                        </v-toolbar>

                        <div class="ml-3" v-if="selectedEvent.start">
                            <h2>Event Details</h2>
                            <br />
                            <v-text-field v-model="modifyEventTitle" :label="selectedEvent.name"></v-text-field>
                            <v-menu ref="menu1"
                                    v-model="menu1"
                                    :close-on-content-click="false"
                                    transition="scale-transition"
                                    offset-y
                                    max-width="290px"
                                    min-width="auto">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-text-field v-model="modifystart"
                                                  label="Start Date"
                                                  persistent-hint
                                                  prepend-icon="mdi-calendar"
                                                  v-bind="attrs"
                                                  @blur="modifystart = parseDate(modstartdateFormatted)"
                                                  v-on="on"></v-text-field>
                                </template>
                                <v-date-picker v-model="modifystart"
                                               no-title
                                               @input="menu1 = false"></v-date-picker>
                            </v-menu>
                            <br />
                            <v-menu ref="menu2"
                                    v-model="menu2"
                                    :close-on-content-click="false"
                                    transition="scale-transition"
                                    offset-y
                                    max-width="290px"
                                    min-width="auto">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-text-field v-model="modifyend"
                                                  label="End Date"
                                                  persistent-hint
                                                  prepend-icon="mdi-calendar"
                                                  v-bind="attrs"
                                                  @blur="modifyend = parseDate(modstartdateFormatted)"
                                                  v-on="on"></v-text-field>
                                </template>
                                <v-date-picker v-model="modifyend"
                                               no-title
                                               @input="menu2 = false"></v-date-picker>
                            </v-menu>
                            <v-menu ref="menu"
                                    v-model="menu3"
                                    :close-on-content-click="false"
                                    :nudge-right="40"
                                    :return-value.sync="time"
                                    transition="scale-transition"
                                    offset-y
                                    max-width="290px"
                                    min-width="290px">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-text-field v-model="time"
                                                  :label="time"
                                                  prepend-icon="mdi-clock-time-four-outline"
                                                  readonly
                                                  v-bind="attrs"
                                                  v-on="on"></v-text-field>
                                </template>
                                <v-time-picker v-if="menu3"
                                               v-model="time"
                                               full-width
                                               use-seconds
                                               @click:second="$refs.menu.save(time)"></v-time-picker>
                            </v-menu>

                            <v-checkbox v-model="checkbox"
                                        label="Recurring Event"></v-checkbox>

                            <h3>Time : </h3>
                        </div>
                        <br />
                        <v-list>
                            <H2 class="ml-3">Attendees</H2>
                            <v-list-item-group color="primary">
                                <v-list-item v-for="(item, i) in selectedEvent.attendees"
                                             :key="i">
                                    {{i+1}}.  {{item.name}}
                                    <v-btn fab
                                           text
                                           small
                                           right
                                           color="red"
                                           @click="removeAttendee(item.id)">
                                        <v-icon small>
                                            mdi-close
                                        </v-icon>
                                    </v-btn>

                                </v-list-item>
                            </v-list-item-group>
                        </v-list>
                        <br />
                        
                        <div class="ml-5">
                            <h3>Add Attendee</h3>
                            <v-row>
                                <v-col cols="8  "><v-text-field v-model="addAttendee" label="Enter Name"></v-text-field></v-col>
                                <v-col cols="4">
                                    <v-btn text
                                           color="primary"
                                           @click="addNewAttendee(selectedEvent.eventId)"
                                           outlined>
                                        Add
                                    </v-btn>
                                </v-col>
                            </v-row>
                        </div>
                        <v-card-actions>
                            <v-btn text
                                   color="secondary"
                                   @click="selectedOpen = false">
                                Cancel
                            </v-btn>
                        </v-card-actions>
                    </v-card>
                </v-dialog>
            </v-sheet>

                
        </v-col>
    </v-row>
</template>

<script>
    import axios from 'axios';
    export default {
        data: vm => ({
            focus: '',
            type: 'month',
            typeToLabel: {
                month: 'Month',
                week: 'Week',
                day: 'Day',
                '4day': '4 Days',
            },
            selectedEvent: {},
            addEventName:'',
            selectedElement: null,
            selectedOpen: false,
            addAttendee: "",
            countAttendee:0,
            events: [],
            events2: [],
            addDialog:false,
            date: (new Date(Date.now() - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10),
            modifystart: '',
            modifyend: '',
            modifyEventTitle:'',
            dateFormatted: vm.formatDate((new Date(Date.now() - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10)),
            modstartdateFormatted: '',
            modenddateFormatted: '',
            time: null,
            menu1: false,
            menu2: false,
            menu3:false,
            data: [],
            colors: ['blue', 'indigo', 'deep-purple', 'cyan', 'green', 'orange', 'grey darken-1'],
            names: ['Meeting', 'Holiday', 'PTO', 'Travel', 'Event', 'Birthday', 'Conference', 'Party'],
        }),
        mounted() {
            this.$refs.calendar.checkChange()
            this.getData();
            this.getAttendeeData();
        },
        computed: {
            computedDateFormatted() {
                return this.formatDate(this.modifystart)
            },
        },

        watch: {
            date() {
                this.modstartdateFormatted = this.formatDate(this.modifystart)
            },
        },

        methods: {
            viewDay({ date }) {
                this.focus = date
                this.type = 'day'
            },
            formatDate(date) {
                if (!date) return null

                const [year, month, day] = date.split('-')
                return `${month}/${day}/${year}`
            },
            parseDate(date) {
                if (!date) return null

                const [month, day, year] = date.split('/')
                return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
            },
            getEventColor(event) {
                return event.color
            },
            setToday() {
                this.focus = ''
            },
            prev() {
                this.$refs.calendar.prev()
            },
            next() {
                this.$refs.calendar.next()
            },
            showEvent({ nativeEvent, event }) {
                const open = () => {
                    this.selectedEvent = event
                    this.modifystart = (new Date(event.start - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10)
                    this.modifyend = (new Date(event.end - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10)
                    this.modstartdateFormatted = this.formatDate((new Date(event.start - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10))
                    this.modenddateFormatted = this.formatDate((new Date(event.start - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10))
                    this.time=event.eventTime
                    this.selectedElement = nativeEvent.target
                    requestAnimationFrame(() => requestAnimationFrame(() => this.selectedOpen = true))
                }

                if (this.selectedOpen) {
                    this.selectedOpen = false
                    requestAnimationFrame(() => requestAnimationFrame(() => open()))
                } else {
                    open()
                }

                nativeEvent.stopPropagation()
            },
            updateRange({ start, end }) {
                const events = []

                const min = new Date(`${start.date}T00:00:00`)
                const max = new Date(`${end.date}T23:59:59`)
                const days = (max.getTime() - min.getTime()) / 86400000
                const eventCount = this.rnd(days, days + 20)

                for (let i = 0; i < eventCount; i++) {
                    const allDay = this.rnd(0, 3) === 0
                    const firstTimestamp = this.rnd(min.getTime(), max.getTime())
                    const first = new Date(firstTimestamp - (firstTimestamp % 900000))
                    const secondTimestamp = this.rnd(2, allDay ? 288 : 8) * 900000
                    const second = new Date(first.getTime() + secondTimestamp)

                    events.push({
                        name: this.names[this.rnd(0, this.names.length - 1)],
                        start: first,
                        end: second,
                        color: this.colors[this.rnd(0, this.colors.length - 1)],
                        timed: !allDay,
                        attendees: [12, 2, 3, 4, 5]
                    })
                }

                this.events = events
            },
            rnd(a, b) {
                return Math.floor((b - a + 1) * Math.random()) + a
            },

            checkattendeejson(arr, value) {
                //let check = false;
                for (let i = 0; i < arr.length; i++) {
                    if (arr[i].eventId == value) {
                        return true;
                    }
                }
                return false;
            },
            async getData() {
                var arr = [];
                let fetchdata = [];
                await fetch("http://localhost:34340/api/calendar/")
                    .then((response) => response.json())
                    .then((data) => (fetchdata = data));
                console.log(fetchdata)
                for (let i = 0; i < fetchdata.length; i++) {
                    if (i == 0) {

                        arr.push({
                            name: fetchdata[i].eventTitle,
                            start: new Date(`${fetchdata[i].startDate}T${fetchdata[i].eventTime}`),
                            end: new Date(`${fetchdata[i].endDate}T00:00:00`),
                            eventTime: fetchdata[i].eventTime,
                            color: this.colors[this.rnd(0, this.colors.length - 1)],
                            eventId: fetchdata[i].eventId,
                            timed: true,
                            attendees: [
                                {
                                    id: fetchdata[i].attendeeid,
                                    name: fetchdata[i].attendeeName,

                                },
                            ],
                        });
                        continue;
                    }
                    if (!this.checkattendeejson(arr, fetchdata[i].eventId)) {
                        arr.push({
                            name: fetchdata[i].eventTitle,
                            start: new Date(`${fetchdata[i].startDate}T${fetchdata[i].eventTime}`),
                            end: new Date(`${fetchdata[i].endDate}T00:00:00`),
                            eventTime: fetchdata[i].eventTime,
                            color: this.colors[this.rnd(0, this.colors.length - 1)],
                            eventId: fetchdata[i].eventId,
                            timed: true,
                            attendees: [
                                {
                                    id: fetchdata[i].attendeeid,
                                    name: fetchdata[i].attendeeName,

                                },
                            ],
                        });
                    } else if (this.checkattendeejson(arr, fetchdata[i].eventId)) {
                        for (let j = 0; j < arr.length; j++) {
                            if (arr[j].eventId == fetchdata[i].eventId) {
                                arr[j].attendees.push({
                                    id: fetchdata[i].attendeeid,
                                    name: fetchdata[i].attendeeName,
                                });
                            }
                        }
                    }
                    this.events2 = arr;

                }
            },
            async getAttendeeData()
            {
                let fetchdata = [];
                await fetch("http://localhost:34340/api/attendees/")
                    .then((response) => response.json())
                    .then((data) => (fetchdata = data));

                this.countAttendee = fetchdata.length;
                console.log(this.countAttendee);
            },
            createEvent()
            {

                var date = new Date();
                var components = [
                    date.getYear(),
                    date.getMonth(),
                    date.getDate(),
                    date.getHours(),
                    date.getMinutes(),
                    date.getSeconds(),
                    date.getMilliseconds()
                ];

                var id = parseInt(components.join("").substring(6, 14));
                console.log(id);
                
                axios.post('http://localhost:34340/api/calendar/', {
                    eventid: id,
                    eventtitle: this.addEventName,
                    startdate: this.modifystart,
                    enddate: this.modifyend,
                    eventtime: this.time

                })
                    .then(function (response) {
                        console.log(response);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });

                axios.post('http://localhost:34340/api/attendees/', {
                    attendeeid: id,
                    eventid: id,
                    attendeename: 'Rohit'

                })
                    .then(function (response) {
                        console.log(response);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
                this.$router.go();
                this.addDialog = false;


            },
            addNewAttendee(id)
            {
                console.log(id);
                
                axios.post('http://localhost:34340/api/attendees/', {
                    attendeeid: this.countAttendee + 1,
                    eventid: id,
                    attendeename: this.addAttendee,

                })
                    .then(function (response) {
                        console.log(response);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });

                this.addAttendee = null;
                this.$router.go();

            },

            modifyEvent(id)
            {
                console.log(id, this.modifyEventTitle, this.modifystart, this.modifyend,this.time);
                
                axios.put('http://localhost:34340/api/calendar/'+id, {
                    eventtitle: this.modifyEventTitle,
                    startdate: this.modifystart,
                    enddate: this.modifyend,
                    eventtime: this.time

                })
                    .then(function (response) {
                        console.log(response);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
                this.addDialog = false;
                this.$router.go();
            },

            deleteEvent(id)
            {
                axios.delete('http://localhost:34340/api/calendar/' + id, {
                    eventtitle: this.modifyEventTitle,
                    startdate: this.modifystart,
                    enddate: this.modifyend,
                    eventtime: this.time

                })
                    .then(function (response) {
                        console.log(response);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
                this.addDialog = false;
                this.$router.go();

            },
            removeAttendee(id)
            {
                axios.delete('http://localhost:34340/api/attendees/' + id, {
                    attendeeid: id,

                })
                    .then(function (response) {
                        console.log(response);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
                this.addDialog = false;
                this.$router.go();
            }


        },
    };
</script>